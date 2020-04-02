using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Services.BillingCycle;
using AcademyCloud.Expenses.Domain.Services.UseCycle;
using AcademyCloud.Expenses.Exceptions;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Billing;
using AcademyCloud.Expenses.Protos.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Services
{
    [Authorize]
    public class BillingService : Billing.BillingBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;
        private BillingCycleService billingCycleService;

        public BillingService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext, BillingCycleService billingCycleService)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
            this.billingCycleService = billingCycleService;
        }

        public override async Task<GetCurrentAllocatedBillingResponse> GetCurrentAllocatedBilling(GetCurrentAllocatedBillingRequest request, ServerCallContext context)
        {
            // The only way to generate database query
            var data = await dbContext.BillingCycleEntries.FirstIfNotNullThrowAsync(
                x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType && x.Id == Guid.Parse(request.Id),
                request.SubjectType, request.Id
            );


            return new GetCurrentAllocatedBillingResponse
            {
                Billing = ToCurrentAllocatedBilling(data)
            };
        }

        private CurrentAllocatedBilling ToCurrentAllocatedBilling(BillingCycleEntry entry)
        {
            return new CurrentAllocatedBilling
            {
                SubjectId = entry.Id.ToString(),
                PayerId = entry.Subject.PayUser.Id.ToString(),
                Quota = entry.Quota.ToGrpc(),
                Amount = billingCycleService.CalculatePrice(entry.Quota),
                NextDue = Timestamp.FromDateTime(billingCycleService.NextDue(entry.LastSettled)),
            };
        }

        public override async Task<GetCurrentAllocatedBillingsResponse> GetCurrentAllocatedBillings(GetCurrentAllocatedBillingsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var data = dbContext.BillingCycleEntries.Where(x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType)
                .AsEnumerable();

            if (request.SubjectType == SubjectType.Project)
            {
                // only the projects under current domain
                var domain = await dbContext.Domains.FindIfNullThrowAsync(tokenClaims.DomainId);
                data = data.Where(x => x.Subject.Project!.Domain == domain);
            }
            else if (request.SubjectType == SubjectType.UserProjectAssignment)
            {
                // only the user project assignment under current project
                if (tokenClaims.ProjectId == null) { throw new RpcException(new Status(StatusCode.PermissionDenied, "Domain User cannot access Users allocated resources.")); }
                var project = await dbContext.Projects.FindIfNullThrowAsync(tokenClaims.ProjectId);
                data = data.Where(x => x.Subject.UserProjectAssignment!.Project == project);
            }

            return new GetCurrentAllocatedBillingsResponse
            {
                Billings = { data.Select(ToCurrentAllocatedBilling) }
            };
        }

        private CurrentUsedBilling ToCurrentUsedBilling(UseCycleEntry entry)
        {
            return new CurrentUsedBilling
            {
                SubjectId = entry.Id.ToString(),
                Resources = entry.Resources.ToGrpc(),
                Amount = billingCycleService.CalculatePrice(entry.Resources),
                NextDue = Timestamp.FromDateTime(billingCycleService.NextDue(entry.LastSettled)),
            };
        }

        public override async Task<GetCurrentUsedBillingResponse> GetCurrentUsedBilling(GetCurrentUsedBillingRequest request, ServerCallContext context)
        {
            var data = await dbContext.UseCycleEntries.FirstIfNotNullThrowAsync(
                x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType && x.Id == Guid.Parse(request.Id),
                request.SubjectType, request.Id
            );

            return new GetCurrentUsedBillingResponse
            {
                Billing = ToCurrentUsedBilling(data)
            };
        }

        public override async Task<GetCurrentUsedBillingsResponse> GetCurrentUsedBillings(GetCurrentUsedBillingsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var data = dbContext.UseCycleEntries.Where(x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType)
                .AsEnumerable();

            if (request.SubjectType == SubjectType.Project)
            {
                // only the projects under current domain
                var domain = await dbContext.Domains.FindIfNullThrowAsync(tokenClaims.DomainId);
                data = data.Where(x => x.Subject.Project!.Domain == domain);
            }
            else if (request.SubjectType == SubjectType.UserProjectAssignment)
            {
                // only the user project assignment under current project
                if (tokenClaims.ProjectId == null) { throw new RpcException(new Status(StatusCode.PermissionDenied, "Domain User cannot access Users allocated resources.")); }
                var project = await dbContext.Projects.FindIfNullThrowAsync(tokenClaims.ProjectId);
                data = data.Where(x => x.Subject.UserProjectAssignment!.Project == project);
            }

            return new GetCurrentUsedBillingsResponse
            {
                Billings = { data.Select(ToCurrentUsedBilling) }
            };
        }

        public override async Task<GetHistoryAllocatedBillingsResponse> GetHistoryAllocatedBillings(GetHistoryAllocatedBillingsRequest request, ServerCallContext context)
        {
            var data = await dbContext.BillingCycleEntries.FirstIfNotNullThrowAsync(
                x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType && x.Id == Guid.Parse(request.Id),
                request.SubjectType, request.Id
            );

            return new GetHistoryAllocatedBillingsResponse
            {
                Billings = {
                    data.Subject.BillingCycleRecords
                        .OrderByDescending(x => x.StartTime)
                        .Select(x => new HistoryAllocatedBilling
                        {
                            Id = x.Id.ToString(),
                            Amount = x.Amount,
                            PayerId = x.OrgTransaction.UserTransaction.Payer!.Id.ToString(),
                            Quota = x.Quota.ToGrpc(),
                            StartTime = x.StartTime.ToGrpc(),
                            EndTime = x.EndTime.ToGrpc(),
                        })
                }
            };
        }

        public override async Task<GetHistoryUsedBillingsResponse> GetHistoryUsedBillings(GetHistoryUsedBillingsRequest request, ServerCallContext context)
        {
            var data = await dbContext.UseCycleEntries.FirstIfNotNullThrowAsync(
                x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType && x.Id == Guid.Parse(request.Id),
                request.SubjectType, request.Id
            );

            return new GetHistoryUsedBillingsResponse
            {
                Billings = {
                    data.Subject.UseCycleRecords
                        .OrderByDescending(x => x.StartTime)
                        .Select(x => new HistoryUsedBilling
                        {
                            Id = x.Id.ToString(),
                            Amount = x.Amount,
                            Resources = x.Resources.ToGrpc(),
                            StartTime = x.StartTime.ToGrpc(),
                            EndTime = x.EndTime.ToGrpc(),
                        })
                }
            };
        }
    }
}
