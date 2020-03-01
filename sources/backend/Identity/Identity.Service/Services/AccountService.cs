using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using AcademyCloud.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services
{
    public class AccountService : Account.AccountBase
    {

        private readonly IdentityDbContext dbContext;
        private readonly JwtSettings jwtSettings;

        public AccountService(IdentityDbContext dbContext, JwtSettings jwtSettings)
        {
            this.dbContext = dbContext;
            this.jwtSettings = jwtSettings;
        }

        [Authorize]
        public override Task<ExitDomainResponse> ExitDomain(ExitDomainRequest request, ServerCallContext context)
        {
            return base.ExitDomain(request, context);
        }

        [Authorize]
        public override Task<GetJoinableDomainsResponse> GetJoinableDomains(GetJoinableDomainsRequest request, ServerCallContext context)
        {
            return base.GetJoinableDomains(request, context);
        }

        [Authorize]
        public override Task<GetJoinedDomainsResponse> GetJoinedDomains(GetJoinedDomainsRequest request, ServerCallContext context)
        {
            return base.GetJoinedDomains(request, context);
        }

        [Authorize]
        public override Task<GetProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
        {
            return base.GetProfile(request, context);
        }

        [Authorize]
        public override Task<JoinDomainResponse> JoinDomain(JoinDomainRequest request, ServerCallContext context)
        {
            return base.JoinDomain(request, context);
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            // Create the user
            var newUser = new User(Guid.NewGuid(), request.Username, request.Password, request.Email, false);
            dbContext.Users.Add(newUser);

            // Create a social project whose project is the new user
            Guid socialDomainId = IdentityDbContext.SocialDomainId;
            var socialDomain = await dbContext.Domains.FirstAsync((domain) => domain.Id == socialDomainId);
            var newProject = new Project(Guid.NewGuid(), request.Username, socialDomain);
            dbContext.Projects.Add(newProject);

            // Assign the user into the project and grant admin role
            var projectAssignment = new UserProjectAssignment(Guid.NewGuid(), newUser, newProject, Domains.ValueObjects.UserRole.Admin);
            dbContext.UserProjectAssignments.Add(projectAssignment);

            // Assign the user into the social domain and grant member role
            var domainAssignment = new Domains.Entities.UserDomainAssignment(Guid.NewGuid(), newUser, socialDomain, Domains.ValueObjects.UserRole.Member);
            dbContext.UserDomainAssignments.Add(domainAssignment);

            // Save changes
            await dbContext.SaveChangesAsync();

            // Return the scope (project scope) and token
            var token = jwtSettings.GenerateToken(new TokenClaims(false, newUser.Id.ToString(), socialDomainId.ToString(), newProject.Id.ToString(), Shared.UserRole.Admin));

            var scope = new Scope()
            {
                System = false,
                Role = UserRole.Admin,
                DomainId = socialDomainId.ToString(),
                DomainName = socialDomain.Name,
                ProjectId = newProject.Id.ToString(),
                ProjectName = newProject.Name,
                Social = true
            };

            return new RegisterResponse()
            {
                Scope = scope,
                Token = token,
            };

        }

        [Authorize]
        public override Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            return base.UpdatePassword(request, context);
        }

        [Authorize]
        public override Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            return base.UpdateProfile(request, context);
        }
    }
}

