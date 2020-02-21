using AcademyCloud.Identity.Auth;
using AcademyCloud.Identity.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        private readonly IdentityDbContext dbContext;
        private ILogger logger;
        private JwtSettings jwtSettings;

        public AuthenticationService(IdentityDbContext dbContext, ILogger logger, JwtSettings jwtSettings)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.jwtSettings = jwtSettings;
        }

        public override async Task<AuthenticationReply> Authenticate(AuthenticationRequest request, ServerCallContext context)
        {
            // find the user
            var user = await dbContext.Users.FirstAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return new AuthenticationReply() { Success = false, Token = null };
            }

            // find whether the user has such scope
            var scope = request.Scope;

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

            // auth successful. generate token accorind to scope
            var claims = new List<Claim>
            {
                new Claim(nameof(scope.UserId), scope.UserId),
                new Claim(nameof(scope.DomainId), scope.DomainId),
                new Claim(nameof(scope.ProjectId), scope.ProjectId),
                new Claim(nameof(scope.Role), scope.Role.ToString())
            };

            var creds = new SigningCredentials(jwtSettings.Key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Issuer,
                claims: claims,
                signingCredentials: creds
                );

            return new AuthenticationReply { Success = false, Token = token.ToString() };

        }

        public override async Task<GetScopesReply> GetScopes(GetScopesRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FirstAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return new GetScopesReply() { Success = false };
            }

            // add projects
            var scopes = user.Projects.Select(x => new Scope()
            {
                DomainId = x.Project.Domain.Id.ToString(),
                ProjectId = x.Project.Id.ToString(),
                UserId = user.Id.ToString(),
                Role = (UserRole)x.Role,
            }).ToList();

            // add domains that are either admin, or not the domain of a project scopes.
            scopes.AddRange(user.Domains
                .Where(x =>
                x.Role == Domains.ValueObjects.UserRole.Admin ||
                scopes.Exists(project => project.DomainId == x.Domain.Id.ToString()))
                .Select(x => new Scope()
                {
                    DomainId = x.Domain.Id.ToString(),
                    ProjectId = null,
                    UserId = user.Id.ToString(),
                    Role = (UserRole)x.Role,
                }));

            var reply = new GetScopesReply() { Success = true };
            reply.Scopes.AddRange(scopes);

            return reply;

        }
    }
}
