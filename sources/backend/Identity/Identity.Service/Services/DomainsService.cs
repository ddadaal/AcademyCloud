using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using AcademyCloud.Identity.Exceptions;
using AcademyCloud.Identity.Extensions;
using AcademyCloud.Identity.Services.Domains;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services.Domains
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

            var assignment = new UserDomainAssignment(Guid.NewGuid(), user, domain, (Identity.Domains.ValueObjects.UserRole)request.Role);

            dbContext.UserDomainAssignments.Add(assignment);

            await dbContext.SaveChangesAsync();

            return new AddUserToDomainResponse { };
        }

        public override async Task<ChangeUserRoleResponse> ChangeUserRole(ChangeUserRoleRequest request, ServerCallContext context)
        {
            var assignment = await dbContext.UserDomainAssignments
                .FirstOrDefaultAsync(x => x.User.Id.ToString() == request.UserId && x.Domain.Id.ToString() == request.DomainId)
                ?? throw EntityNotFoundException.Create<UserDomainAssignment>($"UserId {request.UserId} and DomainId {request.DomainId}");

            assignment.Role = (Identity.Domains.ValueObjects.UserRole)request.Role;

            await dbContext.SaveChangesAsync();

            return new ChangeUserRoleResponse { };
        }

        public override async Task<CreateDomainResponse> CreateDomain(CreateDomainRequest request, ServerCallContext context)
        {
            var payUser = await dbContext.Users.FindIfNullThrowAsync(request.AdminId);
            var domain = new Domain(Guid.NewGuid(), request.Name);
            dbContext.Domains.Add(domain);

            var adminAssignment = new UserDomainAssignment(Guid.NewGuid(), payUser, domain, Identity.Domains.ValueObjects.UserRole.Admin);
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
            var domains = dbContext.Domains.Select(x => new Common.Domain
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Admins = {
                    x.Users.Where(u => u.Role == Identity.Domains.ValueObjects.UserRole.Admin)
                           .Select(u => u.User)
                           .AsEnumerable()
                           .Select(u => u.ToGrpcUser())
                }
            }).AsEnumerable();

            return new GetDomainsResponse
            {
                Domains = { domains }
            };
        }

        public override async Task<GetUsersOfDomainResponse> GetUsersOfDomain(GetUsersOfDomainRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            return new GetUsersOfDomainResponse
            {
                Admins = { domain.Users.Where(x => x.Role == Identity.Domains.ValueObjects.UserRole.Admin).Select(x => x.User.ToGrpcUser()) },
                Members = { domain.Users.Where(x => x.Role == Identity.Domains.ValueObjects.UserRole.Member).Select(x => x.User.ToGrpcUser()) },
            };
        }

        public override async Task<RemoveUserFromDomainResponse> RemoveUserFromDomain(RemoveUserFromDomainRequest request, ServerCallContext context)
        {

            // remove the domain assignment
            var domainAssignment = await dbContext.UserDomainAssignments
                .FirstOrDefaultAsync(x => x.Domain.Id.ToString() == request.DomainId && x.User.Id.ToString() == request.UserId)
                   ?? throw EntityNotFoundException.Create<UserDomainAssignment>($"DomainId {request.DomainId} and UserId {request.UserId}");

            dbContext.UserDomainAssignments.Remove(domainAssignment);

            // remove the project assignment
            var projectAssignments = dbContext.UserProjectAssignments
                .Where(x => x.Project.Domain.Id.ToString() == request.DomainId && x.User.Id.ToString() == request.UserId);

            dbContext.UserProjectAssignments.RemoveRange(projectAssignments);

            await dbContext.SaveChangesAsync();

            return new RemoveUserFromDomainResponse { };
        }

        public override async Task<SetAdminsResponse> SetAdmins(SetAdminsRequest request, ServerCallContext context)
        {
            var domain = await dbContext.Domains.FindIfNullThrowAsync(request.DomainId);

            var assignments = dbContext.UserDomainAssignments.Where(x => x.Domain.Id.ToString() == request.DomainId);

            // 1. Set all roles to members
            await assignments.ForEachAsync(x =>
            {
                x.Role = Identity.Domains.ValueObjects.UserRole.Member;
            });

            // 2. Set the admins to admin. If not in the domain, add them into the admin
            foreach (var userId in request.AdminIds)
            {
                var user = await dbContext.Users.FindIfNullThrowAsync(userId);
                var userAssignment = await assignments.FirstOrDefaultAsync(x => x.User.Id.ToString() == userId);

                if (userAssignment != null)
                {
                    userAssignment.Role = Identity.Domains.ValueObjects.UserRole.Admin;
                }
                else
                {
                    var assignment = new UserDomainAssignment(Guid.NewGuid(), user, domain, Identity.Domains.ValueObjects.UserRole.Admin);
                    dbContext.UserDomainAssignments.Add(assignment);
                }
            }

            await dbContext.SaveChangesAsync();

            return new SetAdminsResponse { };
        }
    }
}