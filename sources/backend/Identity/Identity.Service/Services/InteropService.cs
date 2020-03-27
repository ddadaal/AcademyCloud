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
using AcademyCloud.Identity.Domain.Entities;
using static AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types;

namespace AcademyCloud.Identity.Services
{
    public class InteropService : Interop.InteropBase
    {
        private readonly IdentityDbContext dbContext;

        public InteropService(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private async Task<TItem> Dispatch<TItem>(
            Subject subject,
            TItem system,
            Func<Domain.Entities.Domain, TItem>? domainFunc = null,
            Func<Project, TItem>? projectFunc = null,
            Func<User, TItem>? userFunc = null,
            Func<UserProjectAssignment, TItem>? userProjectAssignmentFunc = null)
        {

            Func<object, TItem> throwFunc = (o) => { throw new ArgumentOutOfRangeException("Type"); };

            return await (subject.Type switch
            {
                SubjectType.Domain => dbContext.Domains.FindIfNullThrowAsync(subject.Id).Then(r => (domainFunc ?? throwFunc)(r)),
                SubjectType.Project => dbContext.Projects.FindIfNullThrowAsync(subject.Id).Then(r => (projectFunc ?? throwFunc)(r)),
                SubjectType.User => dbContext.Users.FindIfNullThrowAsync(subject.Id).Then(r => (userFunc ?? throwFunc)(r)),
                SubjectType.UserProjectAssignment => dbContext.UserProjectAssignments.FindIfNullThrowAsync(subject.Id).Then(r => (userProjectAssignmentFunc ?? throwFunc)(r)),
                SubjectType.System => Task.FromResult(system),
                _ => throw new ArgumentOutOfRangeException(nameof(subject.Type))
            });
        }

        private async Task<Dictionary<string, TItem>> Collect<TItem>(
            IEnumerable<Subject> subjects,
            TItem system,
            Func<Domain.Entities.Domain, TItem>? domainFunc = null,
            Func<Project, TItem>? projectFunc = null,
            Func<User, TItem>? userFunc = null,
            Func<UserProjectAssignment, TItem>? userProjectAssignmentFunc = null)
        {
            var result = new Dictionary<string, TItem>();

            foreach (var x in subjects)
            {
                result[x.Id] = await Dispatch(x, system, domainFunc, projectFunc, userFunc, userProjectAssignmentFunc);
            };

            return result;

        }

        public override async Task<GetNamesResponse> GetNames(GetNamesRequest request, ServerCallContext context)
        {

            return new GetNamesResponse
            {
                IdNameMap =
                {
                    await Collect(
                        request.Subjects,
                        "system",
                        domainFunc: x => x.Name,
                        projectFunc: x => x.Name,
                        userFunc: x => x.Name,
                        userProjectAssignmentFunc: x => x.User.Name
                )}
            };
        }
    }

}
