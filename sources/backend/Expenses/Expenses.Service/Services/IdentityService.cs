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

namespace AcademyCloud.Expenses.Services
{
    public class IdentityService : Identity.IdentityBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;
        private UseCycleTask useCycleTask;
        private BillingCycleTask billingCycleTask;

        public IdentityService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext, UseCycleTask useCycleTask, BillingCycleTask billingCycleTask)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
            this.useCycleTask = useCycleTask;
            this.billingCycleTask = billingCycleTask;
        }

        public override async Task<AddDomainResponse> AddDomain(AddDomainRequest request, ServerCallContext context)
        {
            var payer = await dbContext.Users.FindIfNullThrowAsync(request.PayUserId);
            var system = await dbContext.Systems.FirstOrDefaultAsync();

            var domain = new DomainEntity(Guid.Parse(request.Id), payer, Resources.Zero, system);
            var userDomainAssignment = new UserDomainAssignment(Guid.Parse(request.PayUserAssignmentId), domain, payer);

            dbContext.Domains.Add(domain);
            dbContext.UserDomainAssignments.Add(userDomainAssignment);
            dbContext.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(domain.BillingCycleSubject));

            await dbContext.SaveChangesAsync();
            return new AddDomainResponse { };
        }

        public override async Task<AddProjectResponse> AddProject(AddProjectRequest request, ServerCallContext context)
        {
            var payer = await dbContext.Users.FindIfNullThrowAsync(request.PayUserId);
            var domain = await dbContext.Domains.FindIfNullThrowAsync(tokenClaimsAccessor.TokenClaims.DomainId);

            var project = new Project(Guid.Parse(request.Id), payer, domain, Resources.Zero);
            var payUserProjectAssignment = new UserProjectAssignment(Guid.Parse(request.PayUserAssignmentId), payer, project, Resources.Zero);

            dbContext.Projects.Add(project);
            dbContext.UserProjectAssignments.Add(payUserProjectAssignment);

            dbContext.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(project.BillingCycleSubject));
            dbContext.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(payUserProjectAssignment.BillingCycleSubject));

            dbContext.UseCycleEntries.Add(new Domain.Entities.UseCycle.UseCycleEntry(project.UseCycleSubject));
            dbContext.UseCycleEntries.Add(new Domain.Entities.UseCycle.UseCycleEntry(payUserProjectAssignment.UseCycleSubject));

            await dbContext.SaveChangesAsync();

            return new AddProjectResponse { };
        }

        public override async Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var socialDomain = await dbContext.Domains.FindAsync(SocialDomainId);
            var user = new User(Guid.Parse(request.UserId), 0);
            var project = new Project(Guid.Parse(request.SocialProjectId), user, socialDomain, Resources.Zero);
            var userProjectAssignment = new UserProjectAssignment(Guid.Parse(request.SocialProjectAssignmentId), user, project, Resources.Zero);
            var userDomainAssignment = new UserDomainAssignment(Guid.Parse(request.SocialDomainAssignmentId), socialDomain, user);

            dbContext.Users.Add(user);
            dbContext.Projects.Add(project);
            dbContext.UserProjectAssignments.Add(userProjectAssignment);
            dbContext.UserDomainAssignments.Add(userDomainAssignment);

            // Add user and project into the use cycle, billing cycle and management fee
            dbContext.UseCycleEntries.Add(new Domain.Entities.UseCycle.UseCycleEntry(userProjectAssignment.UseCycleSubject));
            dbContext.UseCycleEntries.Add(new Domain.Entities.UseCycle.UseCycleEntry(project.UseCycleSubject));

            dbContext.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(userProjectAssignment.BillingCycleSubject));
            dbContext.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(project.BillingCycleSubject));

            dbContext.ManagementFeeEntries.Add(new Domain.Entities.ManagementFee.ManagementFeeEntry(project.Payer));
            dbContext.ManagementFeeEntries.Add(new Domain.Entities.ManagementFee.ManagementFeeEntry(user.Payer));

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

        public override Task<AddUserToProjectResponse> AddUserToProject(AddUserToProjectRequest request, ServerCallContext context)
        {
            return base.AddUserToProject(request, context);
        }

        public override async Task<DeleteDomainResponse> DeleteDomain(DeleteDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.Id);
            dbContext.Domains.Remove(domain);

            await dbContext.SaveChangesAsync();

            return new DeleteDomainResponse { };
        }

        public override Task<DeleteProjectResponse> DeleteProject(DeleteProjectRequest request, ServerCallContext context)
        {
            return base.DeleteProject(request, context);
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

        public override Task<RemoveUserFromProjectResponse> RemoveUserFromProject(RemoveUserFromProjectRequest request, ServerCallContext context)
        {
            return base.RemoveUserFromProject(request, context);
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
            billingCycleTask.TrySettle(entry, TransactionReason.DomainQuotaChange);

            // then, change the quota
            domain.Quota = quota.FromGrpc();

            await dbContext.SaveChangesAsync();

            return new SetDomainQuotaResponse { };
        }

        public override Task<SetProjectPayUserResponse> SetProjectPayUser(SetProjectPayUserRequest request, ServerCallContext context)
        {
            return base.SetProjectPayUser(request, context);
        }

        public override Task<SetProjectQuotaResponse> SetProjectQuota(SetProjectQuotaRequest request, ServerCallContext context)
        {
            return base.SetProjectQuota(request, context);
        }
    }
}
