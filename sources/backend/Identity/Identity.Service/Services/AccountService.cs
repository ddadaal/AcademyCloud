using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using AcademyCloud.Identity.Extensions;
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
        public override async Task<ExitDomainResponse> ExitDomain(ExitDomainRequest request, ServerCallContext context)
        {
            var user = context.GetTokenClaims();

            var domainAssignment = await dbContext.UserDomainAssignments
                .FirstAsync(x => x.User.Id.ToString() == user.UserId && x.Domain.Id.ToString() == request.DomainId);

            dbContext.UserDomainAssignments.Remove(domainAssignment);

            var projectAssignments = dbContext.UserProjectAssignments
                .Where(x => x.User.Id.ToString() == user.UserId && x.Project.Domain.Id.ToString() == request.DomainId);

            dbContext.UserProjectAssignments.RemoveRange(projectAssignments);

            await dbContext.SaveChangesAsync();

            return new ExitDomainResponse() { };

        }

        [Authorize]
        public override Task<GetJoinableDomainsResponse> GetJoinableDomains(GetJoinableDomainsRequest request, ServerCallContext context)
        {
            var user = context.GetTokenClaims();

            var alreadyInDomains = dbContext.UserDomainAssignments
                .Where(x => x.User.Id.ToString() == user.UserId)
                .Select(x => x.Domain.Id);

            var notInDomains = dbContext.Domains
                .Where(x => !alreadyInDomains.Contains(x.Id));

            return Task.FromResult(new GetJoinableDomainsResponse()
            {
                Domains = { notInDomains.Select(x => new GetJoinableDomainsResponse.Types.JoinableDomain() { Id = x.Id.ToString(), Name = x.Name }) }
            });


        }

        [Authorize]
        public override Task<GetJoinedDomainsResponse> GetJoinedDomains(GetJoinedDomainsRequest request, ServerCallContext context)
        {
            var user = context.GetTokenClaims();

            var domains = dbContext.UserDomainAssignments
                .Where(x => x.User.Id.ToString() == user.UserId)
                .Select(x => new UserDomainAssignment()
                {
                    DomainId = x.Domain.Id.ToString(),
                    DomainName = x.Domain.Name,
                    Role = (UserRole)x.Role,
                });

            return Task.FromResult(new GetJoinedDomainsResponse()
            {
                Domains = { domains }
            });
        }

        [Authorize]
        public override async Task<GetProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
        {

            var user = context.GetTokenClaims();

            var currentUser = await dbContext.Users.FindAsync(Guid.Parse(user.UserId));

            ExceptionExtensions.ThrowRpcExceptionIfNull(currentUser, user.UserId);

            return new GetProfileResponse()
            {
                Profile = new Profile() { Email = currentUser.Email, Id = currentUser.Id.ToString(), Username = currentUser.Username }
            };
        }

        [Authorize]
        public override async Task<JoinDomainResponse> JoinDomain(JoinDomainRequest request, ServerCallContext context)
        {
            var tokenClaims = context.GetTokenClaims();

            var user = await dbContext.Users.FindAsync(Guid.Parse(tokenClaims.UserId));
            ExceptionExtensions.ThrowRpcExceptionIfNull(user, tokenClaims.UserId);

            var domain = await dbContext.Domains.FindAsync(Guid.Parse(request.DomainId));
            ExceptionExtensions.ThrowRpcExceptionIfNull(domain, request.DomainId);


            var assignment = new Domains.Entities.UserDomainAssignment(Guid.NewGuid(), user, domain, Domains.ValueObjects.UserRole.Member);
            dbContext.UserDomainAssignments.Add(assignment);

            await dbContext.SaveChangesAsync();

            return new JoinDomainResponse() { };
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
        public override async Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            var tokenClaims = context.GetTokenClaims();

            var user = await dbContext.Users.FindAsync(Guid.Parse(tokenClaims.UserId));
            ExceptionExtensions.ThrowRpcExceptionIfNull(user, tokenClaims.UserId);

            if (user.Password != request.Original)
            {
                return new UpdatePasswordResponse() { Result = UpdatePasswordResponse.Types.Result.OriginalNotMatch };
            }

            user.Password = request.Updated;
            await dbContext.SaveChangesAsync();

            return new UpdatePasswordResponse() { Result = UpdatePasswordResponse.Types.Result.Success };
        }

        [Authorize]
        public override async Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            var tokenClaims = context.GetTokenClaims();

            var user = await dbContext.Users.FindAsync(Guid.Parse(tokenClaims.UserId));
            ExceptionExtensions.ThrowRpcExceptionIfNull(user, tokenClaims.UserId);

            user.Email = request.Email;

            await dbContext.SaveChangesAsync();

            return new UpdateProfileResponse() { };
        }
    }
}

