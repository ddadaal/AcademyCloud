using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Authentication;
using static AcademyCloud.Shared.Constants;

namespace AcademyCloud.Identity.Extensions
{
    public static class UserExtensions
    {
        public static IEnumerable<Scope> GetAvailableScopes(this User user)
        {
            // if the user is system user,
            if (user.System)
            {
                return new List<Scope>
                {
                    new Scope()
                    {
                        System = true,
                        DomainId = Guid.Empty.ToString(),
                        DomainName = "System",
                        Role = Protos.Common.UserRole.Admin
                    }
                };
            }

            // add projects
            var scopes = user.Projects.Select(x => new Scope()
            {
                System = false,
                Social = x.Project.Domain.Id == SocialDomainId,
                DomainId = x.Project.Domain.Id.ToString(),
                DomainName = x.Project.Domain.Name,
                ProjectId = x.Project.Id.ToString(),
                ProjectName = x.Project.Name,
                UserProjectAssignmentId = x.Id.ToString(),
                Role = (Protos.Common.UserRole)x.Role,
            }).ToList();

            // add domains
            scopes.AddRange(user.Domains.AsEnumerable()
                .Where(x => x.Domain.Id != SocialDomainId)
                .Select(x => new Scope()
                {
                    System = false,
                    Social = false,
                    DomainId = x.Domain.Id.ToString(),
                    DomainName = x.Domain.Name,
                    Role = (Protos.Common.UserRole)x.Role,
                }));

            return scopes;


        }

        public static Protos.Common.User ToGrpcUser(this User user)
        {
            return new Protos.Common.User
            {
                Id = user.Id.ToString(),
                Name = user.Name,
            };

        }
    }
}
