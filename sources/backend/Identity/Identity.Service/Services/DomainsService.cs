using System;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using AcademyCloud.Identity.Exceptions;
using AcademyCloud.Identity.Extensions;
using AcademyCloud.Identity.Protos.Domains;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AcademyCloud.Identity.Services
{
    [Authorize]
    public class DomainsService : Domains.DomainsBase
    {
        private readonly IdentityDbContext dbContext;

        public DomainsService(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override async Task<AddUserToDomainResponse> AddUserToDomain(AddUserToDomainRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            var assignment = new UserDomainAssignment(Guid.NewGuid(), user, domain, (Domain.ValueObjects.UserRole)request.Role);

            dbContext.UserDomainAssignments.Add(assignment);

            await dbContext.SaveChangesAsync();

            return new AddUserToDomainResponse { };
        }

        public override async Task<ChangeUserRoleResponse> ChangeUserRole(ChangeUserRoleRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            var assignment = await dbContext.UserDomainAssignments
                .FirstOrDefaultAsync(x => x.User == user && x.Domain == domain)
                ?? throw EntityNotFoundException.Create<UserDomainAssignment>($"UserId {request.UserId} and DomainId {request.DomainId}");

            assignment.Role = (Domain.ValueObjects.UserRole)request.Role;

            await dbContext.SaveChangesAsync();

            return new ChangeUserRoleResponse { };
        }

        public override async Task<CreateDomainResponse> CreateDomain(CreateDomainRequest request, ServerCallContext context)
        {
            var payUser = await dbContext.Users.FindIfNullThrowAsync(request.AdminId);
            var domain = new Domain.Entities.Domain(Guid.NewGuid(), request.Name);
            dbContext.Domains.Add(domain);

            var adminAssignment = new UserDomainAssignment(Guid.NewGuid(), payUser, domain, Identity.Domain.ValueObjects.UserRole.Admin);
            dbContext.UserDomainAssignments.Add(adminAssignment);

            await dbContext.SaveChangesAsync();

            return new CreateDomainResponse { };

        }

        public override async Task<DeleteDomainResponse> DeleteDomain(DeleteDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            dbContext.Domains.Remove(domain);

            await dbContext.SaveChangesAsync();

            return new DeleteDomainResponse { };
        }

        public override async Task<GetDomainsResponse> GetDomains(GetDomainsRequest request, ServerCallContext context)
        {
            // load the user domain assignments values first
            var domains = dbContext.Domains.Include(x => x.Users).AsEnumerable();

            var grpcDomains = domains.Select(x => new Protos.Common.Domain
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Admins = {
                    x.Users
                        .Where(u => u.Role == Identity.Domain.ValueObjects.UserRole.Admin)
                        // explicitly load the user value
                        .Select(x => LoadUser(x).ToGrpcUser())
                }
            });

            return new GetDomainsResponse
            {
                Domains = { grpcDomains }
            };
        }

        private Domain.Entities.User LoadUser(UserDomainAssignment assignment)
        {
            dbContext.Entry(assignment).Reference(x => x.User).Load();
            return assignment.User;
        }

        public override async Task<GetUsersOfDomainResponse> GetUsersOfDomain(GetUsersOfDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            // Load users
            dbContext.Entry(domain).Collection(x => x.Users).Load();

            // filter admins and members
            var admins = domain.Users.Where(x => x.Role == Domain.ValueObjects.UserRole.Admin);
            var members = domain.Users.Where(x => x.Role == Domain.ValueObjects.UserRole.Member);

            // explicitly load there users
            return new GetUsersOfDomainResponse
            {
                Admins = { admins.Select(x => LoadUser(x).ToGrpcUser()) },
                Members = { members.Select(x => LoadUser(x).ToGrpcUser()) },
            };
        }

        public override async Task<RemoveUserFromDomainResponse> RemoveUserFromDomain(RemoveUserFromDomainRequest request, ServerCallContext context)
        {

            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);

            // remove the domain assignment
            var domainAssignment = await dbContext.UserDomainAssignments
                .FirstOrDefaultAsync(x => x.Domain == domain && x.User == user)
                   ?? throw EntityNotFoundException.Create<UserDomainAssignment>($"DomainId {request.DomainId} and UserId {request.UserId}");

            dbContext.UserDomainAssignments.Remove(domainAssignment);

            // remove the project assignment
            var projectAssignments = dbContext.UserProjectAssignments
                .Where(x => x.Project.Domain == domain && x.User == user);

            dbContext.UserProjectAssignments.RemoveRange(projectAssignments);

            await dbContext.SaveChangesAsync();

            return new RemoveUserFromDomainResponse { };
        }

        public override async Task<SetAdminsResponse> SetAdmins(SetAdminsRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            var assignments = dbContext.UserDomainAssignments.Where(x => x.Domain == domain);

            // 1. Set all roles to members
            await assignments.ForEachAsync(x =>
            {
                x.Role = Domain.ValueObjects.UserRole.Member;
            });

            // 2. Set the admins to admin. If not in the domain, add them into the admin
            foreach (var userId in request.AdminIds)
            {
                var user = await dbContext.Users.FindIfNullThrowAsync(userId);
                var userAssignment = await assignments.FirstOrDefaultAsync(x => x.User == user);

                if (userAssignment != null)
                {
                    userAssignment.Role = Domain.ValueObjects.UserRole.Admin;
                }
                else
                {
                    var assignment = new UserDomainAssignment(Guid.NewGuid(), user, domain, Domain.ValueObjects.UserRole.Admin);
                    dbContext.UserDomainAssignments.Add(assignment);
                }
            }

            await dbContext.SaveChangesAsync();

            return new SetAdminsResponse { };
        }
    }
}