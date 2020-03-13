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

        private async Task<Dictionary<string, TItem>> Collect<TItem>(
            IEnumerable<Subject> subjects,
            Func<Domain.Entities.System, TItem>? systemFunc = null,
            Func<Domain.Entities.Domain, TItem>? domainFunc = null,
            Func<Project, TItem>? projectFunc = null,
            Func<User, TItem>? userFunc = null,
            Func<UserProjectAssignment, TItem>? userProjectAssignmentFunc = null)
        {

            Func<object, TItem> throwFunc = (o) => { throw new ArgumentOutOfRangeException("Type"); };

            return (await subjects.SelectAsync(x => x.Type switch
            {
                SubjectType.System => dbContext.Systems.FirstAsync().Then(r => (x.Id, (systemFunc ?? throwFunc)(r))),
                SubjectType.Domain => dbContext.Domains.FindIfNullThrowAsync(x.Id).Then(r => (x.Id, (domainFunc ?? throwFunc)(r))),
                SubjectType.Project => dbContext.Projects.FindIfNullThrowAsync(x.Id).Then(r => (x.Id, (projectFunc ?? throwFunc)(r))),
                SubjectType.UserProjectAssignment => dbContext.UserProjectAssignments.FindIfNullThrowAsync(x.Id).Then(r => (x.Id, (userProjectAssignmentFunc ?? throwFunc)(r))),
                SubjectType.User => dbContext.Users.FindIfNullThrowAsync(x.Id).Then(r => (x.Id, (userFunc ?? throwFunc)(r))),
                _ => throw new ArgumentOutOfRangeException(nameof(x.Type))
            }))
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
    }
}
