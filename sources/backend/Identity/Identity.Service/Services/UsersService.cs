using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using AcademyCloud.Identity.Extensions;
using AcademyCloud.Identity.Protos.Users;
using AcademyCloud.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace AcademyCloud.Identity.Services
{
    [Authorize]
    public class UsersService : Users.UsersBase
    {
        private readonly IdentityDbContext dbContext;
        private readonly TokenClaimsAccessor tokenClaimsAccessor;

        public UsersService(IdentityDbContext dbContext, TokenClaimsAccessor tokenClaimsAccessor)
        {
            this.dbContext = dbContext;
            this.tokenClaimsAccessor = tokenClaimsAccessor;
        }

        public override async Task<GetAccessibleUsersResponse> GetAccessibleUsers(GetAccessibleUsersRequest request, ServerCallContext context)
        {
            var claims = tokenClaimsAccessor.TokenClaims;
            IEnumerable<User> users;

            if (claims.IsSystem)
            {
                users = dbContext.Users;
            }
            else if (claims.IsSocial)
            {
                users = new List<User>() { await dbContext.Users.FindAsync(Guid.Parse(claims.UserId)) };
            }
            else if (claims.IsDomainScoped)
            {
                var domain = await dbContext.Domains.FindIfNullThrowAsync(claims.DomainId);
                users = dbContext.UserDomainAssignments.Where(x => x.Domain == domain).Select(x => x.User);
            }
            else if (claims.IsProjectScoped)
            {
                var project = await dbContext.Projects.FindIfNullThrowAsync(claims.ProjectId!);
                users = dbContext.UserProjectAssignments.Where(x => x.Project == project).Select(x => x.User);
            }
            else
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "Unexpected tokenclaims"));
            }

            return new GetAccessibleUsersResponse
            {
                Users = { users.Select(x => x.ToGrpcUser()) }
            };


        }

        [Authorize(AuthPolicy.System)]
        public override async Task<RemoveUserFromSystemResponse> RemoveUserFromSystem(RemoveUserFromSystemRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);

            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return new RemoveUserFromSystemResponse() { };

        }
    }
}
