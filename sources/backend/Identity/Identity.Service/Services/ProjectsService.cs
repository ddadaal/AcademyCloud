using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using AcademyCloud.Identity.Exceptions;
using AcademyCloud.Identity.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Projects;

namespace AcademyCloud.Identity.Services
{
    [Authorize]
    public class ProjectsService : Projects.ProjectsBase
    {
        private readonly IdentityDbContext dbContext;
        private readonly TokenClaimsAccessor tokenClaimsAccessor;

        public ProjectsService(IdentityDbContext dbContext, TokenClaimsAccessor tokenClaimsAccessor)
        {
            this.dbContext = dbContext;
            this.tokenClaimsAccessor = tokenClaimsAccessor;
        }

        public override async Task<AddUserToProjectResponse> AddUserToProject(AddUserToProjectRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);

            var assignment = new UserProjectAssignment(Guid.NewGuid(), user, project, (Domain.ValueObjects.UserRole)request.Role);

            dbContext.UserProjectAssignments.Add(assignment);

            await dbContext.SaveChangesAsync();

            return new AddUserToProjectResponse
            {
                UserProjectAssignmentId = assignment.Id.ToString(),
            };
        }

        public override async Task<ChangeUserRoleResponse> ChangeUserRole(ChangeUserRoleRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);

            var assignment = await dbContext.UserProjectAssignments
                .FirstOrDefaultAsync(x => x.User == user && x.Project == project)
                ?? throw EntityNotFoundException.Create<UserProjectAssignment>($"UserId {request.UserId} and ProjectId {request.ProjectId}");

            assignment.Role = (Domain.ValueObjects.UserRole)request.Role;

            await dbContext.SaveChangesAsync();

            return new ChangeUserRoleResponse { };
        }

        public override async Task<CreateProjectResponse> CreateProject(CreateProjectRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var domain = await dbContext.Domains.FindIfNullThrowAsync(tokenClaims.DomainId);

            var payUser = await dbContext.Users.FindIfNullThrowAsync(request.AdminId);
            var project = new Project(Guid.NewGuid(), request.Name, domain);
            dbContext.Projects.Add(project);

            var adminAssignment = new UserProjectAssignment(Guid.NewGuid(), payUser, project, Domain.ValueObjects.UserRole.Admin);
            dbContext.UserProjectAssignments.Add(adminAssignment);

            await dbContext.SaveChangesAsync();

            return new CreateProjectResponse
            {
                ProjectId = project.Id.ToString(),
                AdminAssignmentId = adminAssignment.Id.ToString(),
            };
        }

        public override async Task<DeleteProjectResponse> DeleteProject(DeleteProjectRequest request, ServerCallContext context)
        {
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);

            dbContext.Projects.Remove(project);

            await dbContext.SaveChangesAsync();

            return new DeleteProjectResponse { };
        }

        private (IEnumerable<Protos.Common.User>, IEnumerable<Protos.Common.User>) GetProjectUsers(Project project)
        {
            var allUsers = project.Users.AsEnumerable();

            var admins = allUsers
                .Where(x => x.Role == Domain.ValueObjects.UserRole.Admin)
                .Select(x => x.User)
                .Select(x => new Protos.Common.User() { Id = x.Id.ToString(), Name = x.Name, Username = x.Username });

            var members = allUsers
                .Where(x => x.Role == Domain.ValueObjects.UserRole.Member)
                .Select(x => x.User)
                .Select(x => new Protos.Common.User() { Id = x.Id.ToString(), Name = x.Name, Username = x.Username });

            return (admins, members);

        }

        public override async Task<GetAccessibleProjectsResponse> GetAccessibleProjects(GetAccessibleProjectsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var domain = await dbContext.Domains.FindIfNullThrowAsync(tokenClaims.DomainId);

            var projects = domain.Projects.Select(x =>
            {
                var (admins, members) = GetProjectUsers(x);

                return new Protos.Common.Project
                {
                    Id = x.Id.ToString(),
                    Admins = { admins },
                    Members = { members },
                    Name = x.Name,
                };
            });

            return new GetAccessibleProjectsResponse
            {
                Projects = { projects }
            };

        }

        public override async Task<GetUsersOfProjectResponse> GetUsersOfProject(GetUsersOfProjectRequest request, ServerCallContext context)
        {
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);

            var (admins, members) = GetProjectUsers(project);

            return new GetUsersOfProjectResponse
            {
                Admins = { admins },
                Members = { members },
            };
        }

        public override async Task<RemoveUserFromProjectResponse> RemoveUserFromProject(RemoveUserFromProjectRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindIfNullThrowAsync(request.UserId);
            var project = await dbContext.Projects.FindIfNullThrowAsync(request.ProjectId);

            var assignment = await dbContext.UserProjectAssignments
                .FirstOrDefaultAsync(x => x.User == user && x.Project == project)
                ?? throw EntityNotFoundException.Create<UserProjectAssignment>($"projectId {request.ProjectId} userId {request.UserId}");

            dbContext.UserProjectAssignments.Remove(assignment);

            await dbContext.SaveChangesAsync();

            return new RemoveUserFromProjectResponse { };
        }
    }
}
