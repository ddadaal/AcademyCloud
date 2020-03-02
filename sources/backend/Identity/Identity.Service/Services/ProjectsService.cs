using AcademyCloud.Identity.Data;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services.Projects
{
    [Authorize]
    public class ProjectsService : Projects.ProjectsBase
    {
        private readonly IdentityDbContext dbContext;

        public ProjectsService(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Task<AddUserToProjectResponse> AddUserToProject(AddUserToProjectRequest request, ServerCallContext context)
        {
            return base.AddUserToProject(request, context); 
        }

        public override Task<ChangeUserRoleResponse> ChangeUserRole(ChangeUserRoleRequest request, ServerCallContext context)
        {
            return base.ChangeUserRole(request, context);
        }

        public override Task<CreateProjectResponse> CreateProject(CreateProjectRequest request, ServerCallContext context)
        {
            return base.CreateProject(request, context);
        }

        public override Task<DeleteProjectResponse> DeleteProject(DeleteProjectRequest request, ServerCallContext context)
        {
            return base.DeleteProject(request, context);
        }

        public override Task<GetAccessibleProjectsResponse> GetAccessibleProjects(GetAccessibleProjectsRequest request, ServerCallContext context)
        {
            return base.GetAccessibleProjects(request, context);
        }

        public override Task<GetUsersOfProjectResponse> GetUsersOfProject(GetUsersOfProjectRequest request, ServerCallContext context)
        {
            return base.GetUsersOfProject(request, context);
        }

        public override Task<RemoveUserFromProjectResponse> RemoveUserFromProject(RemoveUserFromProjectRequest request, ServerCallContext context)
        {
            return base.RemoveUserFromProject(request, context);
        }
    }
}
