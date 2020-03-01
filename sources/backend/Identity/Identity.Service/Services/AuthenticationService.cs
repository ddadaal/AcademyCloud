using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Extensions;
using AcademyCloud.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services.Authentication
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        private readonly IdentityDbContext dbContext;
        private JwtSettings jwtSettings;

        public AuthenticationService(IdentityDbContext dbContext, JwtSettings jwtSettings)
        {
            this.dbContext = dbContext;
            this.jwtSettings = jwtSettings;
        }

        public override async Task<AuthenticationReply> Authenticate(AuthenticationRequest request, ServerCallContext context)
        {
            var scope = request.Scope;

            // find the user
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return new AuthenticationReply() { Success = false };
            }

            if (scope.System)
            {
                if (!user.System)
                {
                    return new AuthenticationReply { Success = false };
                }
            }
            // In gRPC there is no null value, the empty string means nothing
            else if (string.IsNullOrEmpty(scope.ProjectId))
            {
                // it's a domain scope, find whether the user has it
                var domain = user.Domains.FirstOrDefault(domain => (int)domain.Role == (int)scope.Role);
                if (domain == null)
                {
                    return new AuthenticationReply { Success = false };
                }
            }
            else
            {
                var project = user.Projects.FirstOrDefault(project => (int)project.Role == (int)scope.Role);
                if (project == null)
                {
                    return new AuthenticationReply { Success = false };
                }
            }

            // auth successful. generate token accorind to token claims
            var claims = new TokenClaims(scope.System, user.Id.ToString(), scope.DomainId, scope.ProjectId, (Shared.UserRole)scope.Role);

            return new AuthenticationReply
            {
                Success = true,
                Token = jwtSettings.GenerateToken(claims),
            };

        }

        public override async Task<GetScopesReply> GetScopes(GetScopesRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return new GetScopesReply() { Success = false };
            }

            return new GetScopesReply() { Success = true, Scopes = { user.GetAvailableScopes() } };

        }

        [Authorize]
        public override Task<GetTokenInfoReply> GetTokenInfo(GetTokenInfoRequest request, ServerCallContext context)
        {

            var tokenClaims = context.GetTokenClaims();

            return Task.FromResult(new GetTokenInfoReply()
            {
                UserId = tokenClaims.UserId,
                Scope = new Scope()
                {
                    System = tokenClaims.System,
                    DomainId = tokenClaims.DomainId,
                    ProjectId = tokenClaims.ProjectId,
                    Role = (UserRole)tokenClaims.Role,
                }
            });
        }
    }
}
