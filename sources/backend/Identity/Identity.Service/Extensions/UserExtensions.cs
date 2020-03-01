using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using AcademyCloud.Identity.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        Role = Services.Common.UserRole.Admin
                    }
                };
            }

            // add projects
            var scopes = user.Projects.Select(x => new Scope()
            {
                System = false,
                Social = x.Project.Domain.Id == IdentityDbContext.SocialDomainId,
                DomainId = x.Project.Domain.Id.ToString(),
                DomainName = x.Project.Domain.Name,
                ProjectId = x.Project.Id.ToString(),
                ProjectName = x.Project.Name,
                Role = (Services.Common.UserRole)x.Role,
            }).ToList();

            // load 

            // add domains that are either admin, or not the domain of a project scopes.
            scopes.AddRange(user.Domains.AsEnumerable()
                .Where(x =>
                    x.Role == Domains.ValueObjects.UserRole.Admin ||
                    !scopes.Exists(project => project.DomainId == x.Domain.Id.ToString())
                )
                .Select(x => new Scope()
                {
                    System = false,
                    Social = x.Domain.Id == IdentityDbContext.SocialDomainId,
                    DomainId = x.Domain.Id.ToString(),
                    DomainName = x.Domain.Name,
                    Role = (Services.Common.UserRole)x.Role,
                }));

            return scopes;


        }
    }
}
