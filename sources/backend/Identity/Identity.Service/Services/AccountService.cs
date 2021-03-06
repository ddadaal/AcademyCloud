﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using AcademyCloud.Identity.Extensions;
using AcademyCloud.Identity.Protos.Account;
using AcademyCloud.Identity.Protos.Authentication;
using AcademyCloud.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static AcademyCloud.Shared.Constants;

namespace AcademyCloud.Identity.Services
{
    public class AccountService : Account.AccountBase
    {

        private readonly IdentityDbContext dbContext;
        private readonly JwtSettings jwtSettings;
        private readonly TokenClaimsAccessor tokenClaimsAccessor;

        public AccountService(IdentityDbContext dbContext, JwtSettings jwtSettings, TokenClaimsAccessor tokenClaimsAccessor)
        {
            this.dbContext = dbContext;
            this.jwtSettings = jwtSettings;
            this.tokenClaimsAccessor = tokenClaimsAccessor;
        }

        [Authorize]
        public override async Task<GetScopesResponse> GetScopes(Protos.Account.GetScopesRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            return new GetScopesResponse { Scopes = { user.GetAvailableScopes() } };
        }

        [Authorize]
        public override async Task<ExitDomainResponse> ExitDomain(ExitDomainRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);
            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var domainAssignment = await dbContext.UserDomainAssignments
                .FirstAsync(x => x.User == user && x.Domain == domain);

            dbContext.UserDomainAssignments.Remove(domainAssignment);

            var projectAssignments = dbContext.UserProjectAssignments
                .Where(x => x.User == user && x.Project.Domain == domain);

            dbContext.UserProjectAssignments.RemoveRange(projectAssignments);

            await dbContext.SaveChangesAsync();

            return new ExitDomainResponse() { };

        }

        [Authorize]
        public override async Task<GetJoinableDomainsResponse> GetJoinableDomains(GetJoinableDomainsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var alreadyInDomains = dbContext.UserDomainAssignments
                .Where(x => x.User == user)
                .Select(x => x.Domain.Id);

            var notInDomains = dbContext.Domains
                .Where(x => !alreadyInDomains.Contains(x.Id));

            return new GetJoinableDomainsResponse()
            {
                Domains = { notInDomains.Select(x => new GetJoinableDomainsResponse.Types.JoinableDomain() { Id = x.Id.ToString(), Name = x.Name }) }
            };


        }

        [Authorize]
        public override async Task<GetJoinedDomainsResponse> GetJoinedDomains(GetJoinedDomainsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;
            var currentUser = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var domains = dbContext.UserDomainAssignments
                .Where(x => x.User == currentUser)
                .Select(x => new Protos.Account.UserDomainAssignment()
                {
                    DomainId = x.Domain.Id.ToString(),
                    DomainName = x.Domain.Name,
                    Role = (Protos.Common.UserRole)x.Role,
                });

            return new GetJoinedDomainsResponse()
            {
                Domains = { domains }
            };
        }

        [Authorize]
        public override async Task<GetProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
        {

            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var currentUser = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            return new GetProfileResponse()
            {
                Profile = new Profile() { Email = currentUser.Email, Id = currentUser.Id.ToString(), Username = currentUser.Username, Name = currentUser.Name }
            };
        }

        [Authorize]
        public override async Task<JoinDomainResponse> JoinDomain(JoinDomainRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            var assignment = new Identity.Domain.Entities.UserDomainAssignment(Guid.NewGuid(), user, domain, Domain.ValueObjects.UserRole.Member);

            dbContext.UserDomainAssignments.Add(assignment);

            await dbContext.SaveChangesAsync();

            return new JoinDomainResponse() { };
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            // Create the user
            var newUser = new User(Guid.NewGuid(), request.Username, request.Username, request.Password, request.Email, false);
            dbContext.Users.Add(newUser);

            // Create a social project whose project name is the new user
            Guid socialDomainId = SocialDomainId;
            var socialDomain = await dbContext.Domains.FirstAsync((domain) => domain.Id == socialDomainId);
            var newProject = new Project(Guid.NewGuid(), request.Username, socialDomain);
            dbContext.Projects.Add(newProject);

            // Assign the user into the project and grant admin role
            var projectAssignment = new UserProjectAssignment(Guid.NewGuid(), newUser, newProject, Identity.Domain.ValueObjects.UserRole.Admin);
            dbContext.UserProjectAssignments.Add(projectAssignment);

            // Assign the user into the social domain and grant member role
            var domainAssignment = new Identity.Domain.Entities.UserDomainAssignment(Guid.NewGuid(), newUser, socialDomain, Identity.Domain.ValueObjects.UserRole.Member);
            dbContext.UserDomainAssignments.Add(domainAssignment);

            // Save changes
            await dbContext.SaveChangesAsync();

            // Return the scope (project scope) and token
            var token = jwtSettings.GenerateToken(new TokenClaims(false, true, newUser.Id.ToString(), socialDomainId.ToString(), newProject.Id.ToString(), projectAssignment.Id.ToString(), Shared.UserRole.Admin));

            var scope = new Scope()
            {
                System = false,
                Role = Protos.Common.UserRole.Admin,
                DomainId = socialDomainId.ToString(),
                DomainName = socialDomain.Name,
                ProjectId = newProject.Id.ToString(),
                ProjectName = newProject.Name,
                UserProjectAssignmentId = projectAssignment.Id.ToString(),
                Social = true
            };

            return new RegisterResponse()
            {
                UserId = newUser.Id.ToString(),
                SocialProjectId = newProject.Id.ToString(),
                SocialDomainAssignmentId = domainAssignment.Id.ToString(),
                SocialProjectAssignmentId = projectAssignment.Id.ToString(),
                Scope = scope,
                Token = token,
            };

        }

        [Authorize]
        public override async Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

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
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            user.Email = request.Email;
            user.Name = request.Name;

            await dbContext.SaveChangesAsync();

            return new UpdateProfileResponse() { };
        }
    }
}

