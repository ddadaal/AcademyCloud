using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Data;
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
        private UseCycleTask useCycleTask;
        private BillingCycleTask billingCycleTask;

        public BillingService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext, UseCycleTask useCycleTask, BillingCycleTask billingCycleTask)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
            this.useCycleTask = useCycleTask;
            this.billingCycleTask = billingCycleTask;
        }

        public override async Task<GetCurrentAllocatedBillingResponse> GetCurrentAllocatedBilling(GetCurrentAllocatedBillingRequest request, ServerCallContext context)
        {
            // The only way to generate database query
            var data = dbContext.BillingCycleEntries
                .First(x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType && x.Id == Guid.Parse(request.Id));

            return new GetCurrentAllocatedBillingResponse
            {
                Billing = new CurrentAllocatedBilling
                {
                    SubjectId = request.Id,
                    PayerId = data.Subject.PayUser.Id.ToString(),
                    Resources = data.Quota.ToGrpc(),
                    Amount = billingCycleTask.CalculatePrice(data.Quota),
                    NextDue = Timestamp.FromDateTime(billingCycleTask.NextDue(data.LastSettled)),
                }
            };
        }

        public override async Task<GetCurrentAllocatedBillingsResponse> GetCurrentAllocatedBillings(GetCurrentAllocatedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetCurrentAllocatedBillings(request, context);
        }

        public override async Task<GetCurrentUsedBillingResponse> GetCurrentUsedBilling(GetCurrentUsedBillingRequest request, ServerCallContext context)
        {
            var data = dbContext.UseCycleEntries
                .First(x => x.SubjectType == (Domain.ValueObjects.SubjectType)request.SubjectType && x.Id == Guid.Parse(request.Id));

            return new GetCurrentUsedBillingResponse
            {
                Billing = new CurrentUsedBilling
                {
                    SubjectId = request.Id,
                    Resources = data.Resources.ToGrpc(),
                    Amount = billingCycleTask.CalculatePrice(data.Resources),
                    NextDue = Timestamp.FromDateTime(billingCycleTask.NextDue(data.LastSettled)),
                }
            };
        }

        public override async Task<GetCurrentUsedBillingsResponse> GetCurrentUsedBillings(GetCurrentUsedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetCurrentUsedBillings(request, context);
        }

        public override async Task<GetHistoryAllocatedBillingsResponse> GetHistoryAllocatedBillings(GetHistoryAllocatedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetHistoryAllocatedBillings(request, context);
        }

        public override async Task<GetHistoryUsedBillingsResponse> GetHistoryUsedBillings(GetHistoryUsedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetHistoryUsedBillings(request, context);
        }
    }
}
