using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Protos.Interop;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Shared;
using AcademyCloud.Identity.Extensions;
using SubjectType = AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.SubjectType;

namespace AcademyCloud.Identity.Services
{
    public class InteropService : Interop.InteropBase
    {
        private readonly IdentityDbContext dbContext;

        public InteropService(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override async Task<GetNamesResponse> GetNames(GetNamesRequest request, ServerCallContext context)
        {
            var result = new GetNamesResponse
            {
                IdNameMap = { }
            };

            await request.Subjects.SelectAsync(x => x.Type switch 
            {
                SubjectType.Domain => dbContext.Domains.FindIfNullThrowAsync(x.Id).Then(r => result.IdNameMap[x.Id] = r.Name),
                SubjectType.Project => dbContext.Projects.FindIfNullThrowAsync(x.Id).Then(r => result.IdNameMap[x.Id] = r.Name),
                SubjectType.User => dbContext.Users.FindIfNullThrowAsync(x.Id).Then(r => result.IdNameMap[x.Id] = r.Name),
                SubjectType.System => Task.FromResult(result.IdNameMap[x.Id] = "system"),
            });

            return result;
        }
    }

}
