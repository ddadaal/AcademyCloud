﻿using AcademyCloud.Identity.Data;
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

namespace AcademyCloud.Identity.Services
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
                return new AuthenticationReply() { Success = false, Token = null };
            }

            if (scope.System && !user.System)
            {
                return new AuthenticationReply { Success = false, Token = null };
            }
            if (scope.ProjectId == null)
            {
                // it's a domain scope, find whether the user has it
                var domain = user.Domains.First(domain => (int)domain.Role == (int)scope.Role);
                if (domain == null)
                {
                    return new AuthenticationReply { Success = false, Token = null };
                }
            }
            else
            {
                var project = user.Projects.First(project => (int)project.Role == (int)scope.Role);
                if (project == null)
                {
                    return new AuthenticationReply { Success = false, Token = null };
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

            // if the user is system user,
            if (user.System)
            {
                return new GetScopesReply()
                {
                    Scopes = {
                        new Scope()
                        {
                            System = true,
                            DomainId = Guid.Empty.ToString(),
                            DomainName = "System",
                            Role = UserRole.Admin
                        }
                    },
                    Success = true,
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
                Role = (UserRole)x.Role,
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
                    Role = (UserRole)x.Role,
                }));

            return new GetScopesReply() { Success = true, Scopes = { scopes } };

        }

        [Authorize]
        public override Task<GetTokenInfoReply> GetTokenInfo(GetTokenInfoRequest request, ServerCallContext context)
        {
            var claims = context.GetHttpContext().User;

            var tokenClaims = TokenClaims.FromClaimPrincinpal(claims);

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
