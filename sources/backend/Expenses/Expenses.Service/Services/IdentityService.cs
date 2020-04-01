using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Identity;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainEntity = AcademyCloud.Expenses.Domain.Entities.Domain;
using static AcademyCloud.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using AcademyCloud.Expenses.Domain.Services.ManagementFee;
using AcademyCloud.Expenses.Domain.Services.BillingCycle;
using AcademyCloud.Expenses.Domain.Services.UseCycle;

namespace AcademyCloud.Expenses.Services
{
    public class IdentityService : Identity.IdentityBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;
        private BillingCycleService billingCycleService;

        public IdentityService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext, BillingCycleService billingCycleService)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
            this.billingCycleService = billingCycleService;
        }

        public override async Task<AddDomainResponse> AddDomain(AddDomainRequest request, ServerCallContext context)
        {
            var payer = await dbContext.Users.FindIfNullThrowAsync(request.PayUserId);
            var system = await dbContext.Systems.FirstOrDefaultAsync();

            var domain = new DomainEntity(Guid.Parse(request.Id), payer, Resources.Zero, system);
            var userDomainAssignment = new UserDomainAssignment(Guid.Parse(request.PayUserAssignmentId), domain, payer);

            dbContext.Domains.Add(domain);
            dbContext.UserDomainAssignments.Add(userDomainAssignment);

            dbContext.BillingCycleEntries.Add(new BillingCycleEntry(domain.BillingCycleSubject));
            dbContext.UseCycleEntries.Add(new UseCycleEntry(domain.UseCycleSubject));
            dbContext.ManagementFeeEntries.Add(new ManagementFeeEntry(domain.Payer));

            await dbContext.SaveChangesAsync();
            return new AddDomainResponse { };
        }

        [Authorize]
        public override async Task<AddProjectResponse> AddProject(AddProjectRequest request, ServerCallContext context)
        {
            var payer = await dbContext.Users.FindIfNullThrowAsync(request.PayUserId);
            var domain = await dbContext.Domains.FindIfNullThrowAsync(tokenClaimsAccessor.TokenClaims.DomainId);

            var project = new Project(Guid.Parse(request.Id), payer, domain, Resources.Zero);
            var payUserProjectAssignment = new UserProjectAssignment(Guid.Parse(request.PayUserAssignmentId), payer, project, Resources.Zero);

            dbContext.Projects.Add(project);
            dbContext.UserProjectAssignments.Add(payUserProjectAssignment);

            dbContext.BillingCycleEntries.Add(new BillingCycleEntry(project.BillingCycleSubject));
            dbContext.BillingCycleEntries.Add(new BillingCycleEntry(payUserProjectAssignment.BillingCycleSubject));

            dbContext.UseCycleEntries.Add(new UseCycleEntry(project.UseCycleSubject));
            dbContext.UseCycleEntries.Add(new UseCycleEntry(payUserProjectAssignment.UseCycleSubject));

            dbContext.ManagementFeeEntries.Add(new ManagementFeeEntry(project.Payer));

            await dbContext.SaveChangesAsync();

            return new AddProjectResponse { };
        }

        public override async Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var socialDomain = await dbContext.Domains.FindAsync(SocialDomainId);
            var user = new User(Guid.Parse(request.UserId), 0);
            // Set the project and user with their initial quota
            var project = new Project(Guid.Parse(request.SocialProjectId), user, socialDomain, Resources.QuotaForSocialProject);
            var userProjectAssignment = new UserProjectAssignment(Guid.Parse(request.SocialProjectAssignmentId), user, project, Resources.QuotaForSocialProject);
            var userDomainAssignment = new UserDomainAssignment(Guid.Parse(request.SocialDomainAssignmentId), socialDomain, user);

            dbContext.Users.Add(user);
            dbContext.Projects.Add(project);
            dbContext.UserProjectAssignments.Add(userProjectAssignment);
            dbContext.UserDomainAssignments.Add(userDomainAssignment);

            // Add user and project into the use cycle, billing cycle and management fee
            dbContext.UseCycleEntries.Add(new UseCycleEntry(userProjectAssignment.UseCycleSubject));
            dbContext.UseCycleEntries.Add(new UseCycleEntry(project.UseCycleSubject));

            dbContext.BillingCycleEntries.Add(new BillingCycleEntry(userProjectAssignment.BillingCycleSubject));
            dbContext.BillingCycleEntries.Add(new BillingCycleEntry(project.BillingCycleSubject));

            dbContext.ManagementFeeEntries.Add(new ManagementFeeEntry(project.Payer));
            dbContext.ManagementFeeEntries.Add(new ManagementFeeEntry(user.Payer));

            await dbContext.SaveChangesAsync();

            return new AddUserResponse { };
        }

        public override async Task<AddUserToDomainResponse> AddUserToDomain(AddUserToDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);

            dbContext.UserDomainAssignments.Add(new UserDomainAssignment(Guid.Parse(request.UserDomainAssignmentId), domain, user));

            await dbContext.SaveChangesAsync();

            return new AddUserToDomainResponse { };

        }

        public override async Task<AddUserToProjectResponse> AddUserToProject(AddUserToProjectRequest request, ServerCallContext context)
        {
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var userProjectAssignment = new UserProjectAssignment(Guid.Parse(request.UserProjectAssignmentId), user, project, Resources.Zero);

            dbContext.UserProjectAssignments.Add(userProjectAssignment);

            dbContext.BillingCycleEntries.Add(new BillingCycleEntry(userProjectAssignment.BillingCycleSubject));
            dbContext.UseCycleEntries.Add(new UseCycleEntry(userProjectAssignment.UseCycleSubject));

            await dbContext.SaveChangesAsync();

            return new AddUserToProjectResponse { };
        }

        public override async Task<DeleteDomainResponse> DeleteDomain(DeleteDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.Id);
            dbContext.Domains.Remove(domain);
            
            // delete of entries should happen automatically by EFCore with foreign key constraints.

            await dbContext.SaveChangesAsync();

            return new DeleteDomainResponse { };
        }

        public override async Task<DeleteProjectResponse> DeleteProject(DeleteProjectRequest request, ServerCallContext context)
        {
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.Id);
            dbContext.Projects.Remove(project);

            await dbContext.SaveChangesAsync();

            return new DeleteProjectResponse { };
        }

        public override async Task<RemoveUserFromDomainResponse> RemoveUserFromDomain(RemoveUserFromDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);

            var assignment = await dbContext.UserDomainAssignments.FirstIfNotNullThrowAsync(x => x.Domain == domain && x.User == user);

            dbContext.UserDomainAssignments.Remove(assignment);

            await dbContext.SaveChangesAsync();

            return new RemoveUserFromDomainResponse { };
        }

        public override async Task<RemoveUserFromProjectResponse> RemoveUserFromProject(RemoveUserFromProjectRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);

            var assignment = await dbContext.UserProjectAssignments.FirstIfNotNullThrowAsync(x => x.Project == domain && x.User == user);

            dbContext.UserProjectAssignments.Remove(assignment);

            await dbContext.SaveChangesAsync();

            return new RemoveUserFromProjectResponse { };
        }

        public override async Task<SetDomainPayUserResponse> SetDomainPayUser(SetDomainPayUserRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.PayUserId);

            domain.PayUser = user;

            await dbContext.SaveChangesAsync();

            return new SetDomainPayUserResponse { };

        }

        public override async Task<SetDomainQuotaResponse> SetDomainQuota(SetDomainQuotaRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);
            var quota = request.Quota;

            // first, settle billing cycle

            // BillingCycleEntry share id with its subject.
            var entry = await dbContext.BillingCycleEntries.FindIfNullThrowAsync(request.DomainId);
            billingCycleService.TrySettle(entry, TransactionReason.DomainQuotaChange);

            // then, change the quota
            domain.Quota = quota.FromGrpc();

            await dbContext.SaveChangesAsync();

            return new SetDomainQuotaResponse { };
        }

        public override async Task<SetProjectPayUserResponse> SetProjectPayUser(SetProjectPayUserRequest request, ServerCallContext context)
        {
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.PayUserId);

            project.PayUser = user;

            await dbContext.SaveChangesAsync();

            return new SetProjectPayUserResponse { };
        }

        public override async Task<SetProjectQuotaResponse> SetProjectQuota(SetProjectQuotaRequest request, ServerCallContext context)
        {
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);
            var quota = request.Quota;

            // first, settle billing cycle

            // BillingCycleEntry share id with its subject.
            var entry = await dbContext.BillingCycleEntries.FindIfNullThrowAsync(request.ProjectId);
            billingCycleService.TrySettle(entry, TransactionReason.ProjectQuotaChange);

            // then, change the quota
            project.Quota = quota.FromGrpc();

            await dbContext.SaveChangesAsync();

            return new SetProjectQuotaResponse { };
        }

        public override async Task<SetProjectUserQuotaResponse> SetProjectUserQuota(SetProjectUserQuotaRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);
            var quota = request.Quota;

            var userAssignment = await dbContext.UserProjectAssignments.FirstIfNotNullThrowAsync(x => x.User == user && x.Project == project);

            // BillingCycleEntry share id with its subject.
            var entry = await dbContext.BillingCycleEntries.FindIfNullThrowAsync(userAssignment.Id);
            billingCycleService.TrySettle(entry, TransactionReason.ProjectQuotaChange);

            // then, change the quota
            userAssignment.Quota = quota.FromGrpc();

            await dbContext.SaveChangesAsync();

            return new SetProjectUserQuotaResponse { };

        }

        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return new DeleteUserResponse { };
        }
    }
}
