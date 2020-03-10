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
        public override async Task<GetActivityResponse> GetActivity(GetActivityRequest request, ServerCallContext context)
        {
            var result = new GetActivityResponse
            {
                Activities = { }
            };

            await request.Subjects.SelectAsync(x => x.Type switch 
            {
                SubjectType.Domain => dbContext.Domains.FindIfNullThrowAsync(x.Id).Then(r => result.Activities[x.Id] = r.Active),
                SubjectType.Project => dbContext.Projects.FindIfNullThrowAsync(x.Id).Then(r => result.Activities[x.Id] = r.Active),
                SubjectType.User => dbContext.Users.FindIfNullThrowAsync(x.Id).Then(r => result.Activities[x.Id] = r.Active),
                SubjectType.System => Task.FromResult(result.Activities[x.Id] = true),
            });

            return result;
        }
    }
}
