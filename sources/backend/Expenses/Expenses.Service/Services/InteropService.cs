using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Interop;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Shared;
using AcademyCloud.Expenses.Protos.Common;
using AcademyCloud.Expenses.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;
using AcademyCloud.Expenses.Domain.Entities;

namespace AcademyCloud.Expenses.Services
{
    public class InteropService : Interop.InteropBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;

        public InteropService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
        }

        private void ThrowFunc<T>(T o)
        {
            throw new ArgumentOutOfRangeException(nameof(o));
        }

        private async Task<TItem> Dispatch<TItem>(
            Subject subject,
            Func<Domain.Entities.System, TItem>? systemFunc = null,
            Func<Domain.Entities.Domain, TItem>? domainFunc = null,
            Func<Project, TItem>? projectFunc = null,
            Func<User, TItem>? userFunc = null,
            Func<UserProjectAssignment, TItem>? userProjectAssignmentFunc = null)
        {

            Func<object, TItem> throwFunc = (o) => { throw new ArgumentOutOfRangeException("Type"); };

            return await (subject.Type switch
            {
                SubjectType.System => dbContext.Systems.FirstAsync().Then(r => (systemFunc ?? throwFunc)(r)),
                SubjectType.Domain => dbContext.Domains.FindIfNullThrowAsync(subject.Id).Then(r => (domainFunc ?? throwFunc)(r)),
                SubjectType.Project => dbContext.Projects.FindIfNullThrowAsync(subject.Id).Then(r => (projectFunc ?? throwFunc)(r)),
                SubjectType.UserProjectAssignment => dbContext.UserProjectAssignments.FindIfNullThrowAsync(subject.Id).Then(r => ((userProjectAssignmentFunc ?? throwFunc)(r))),
                SubjectType.User => dbContext.Users.FindIfNullThrowAsync(subject.Id).Then(r => (userFunc ?? throwFunc)(r)),
                _ => throw new ArgumentOutOfRangeException(nameof(subject.Type))
            });
        }

        private async Task<Dictionary<string, TItem>> Collect<TItem>(
            IEnumerable<Subject> subjects,
            Func<Domain.Entities.System, TItem>? systemFunc = null,
            Func<Domain.Entities.Domain, TItem>? domainFunc = null,
            Func<Project, TItem>? projectFunc = null,
            Func<User, TItem>? userFunc = null,
            Func<UserProjectAssignment, TItem>? userProjectAssignmentFunc = null)
        {

            return (await subjects.SelectAsync(async x => (x.Id, await Dispatch(x, systemFunc, domainFunc, projectFunc, userFunc, userProjectAssignmentFunc))))
            .ToDictionary(x => x.Id, x => x.Item2);

        }

        public override async Task<GetActivityResponse> GetActivity(GetActivityRequest request, ServerCallContext context)
        {

            return new GetActivityResponse
            {
                Activities =
                {
                    await Collect(
                        request.Subjects,
                        domainFunc: x => x.Active,
                        projectFunc: x => x.Active,
                        userFunc: x => x.Active,
                        systemFunc: x => true,
                        userProjectAssignmentFunc: x => x.Active)
                }
            };

        }

        public override async Task<GetPayUserResponse> GetPayUser(GetPayUserRequest request, ServerCallContext context)
        {
            return new GetPayUserResponse
            {
                PayUsers =
                {
                    await Collect(request.Subjects,
                        systemFunc: x => x.ReceiveUser.Id.ToString(),
                        domainFunc: x => x.PayUser.Id.ToString(),
                        projectFunc: x => x.PayUser.Id.ToString(),
                        userProjectAssignmentFunc: x => x.PayUser.Id.ToString()
                    )
                }
            };
        }

        public override async Task<GetQuotaResponse> GetQuota(GetQuotaRequest request, ServerCallContext context)
        {
            return new GetQuotaResponse
            {
                Quotas =
                {
                    await Collect(request.Subjects,
                        domainFunc: x => x.Quota.ToGrpc(),
                        projectFunc: x => x.Quota.ToGrpc(),
                        userProjectAssignmentFunc: x => x.Quota.ToGrpc()
                    )
                }
            };
        }

        public override async Task<GetQuotaStatusResponse> GetQuotaStatus(GetQuotaStatusRequest request, ServerCallContext context)
        {
            var totalSystemResources = new Domain.ValueObjects.Resources(1000, 1000, 1000);
            var (total, used) = await Dispatch(request.Subject,
                systemFunc: x => (totalSystemResources, Domain.ValueObjects.Resources.Sum(x.Domains.Select(x => x.Quota))),
                domainFunc: x => (x.Quota, Domain.ValueObjects.Resources.Sum(x.Projects.Select(x => x.Quota))),
                projectFunc: x => (x.Quota, Domain.ValueObjects.Resources.Sum(x.Users.Select(x => x.Quota)))
                );

            return new GetQuotaStatusResponse
            {
                Total = total.ToGrpc(),
                Used = used.ToGrpc(),
            };

        }
    }
}
