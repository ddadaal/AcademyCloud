using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Identity.Projects;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AcademyCloud.API.Utils.Dummies;

namespace AcademyCloud.API.Controllers.Identity
{
    [Route("/identity/projects")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ServiceClientFactory factory;

        public ProjectsController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public async Task<GetAccessibleProjectsResponse> GetAccessibleProjects()
        {

            var resp = await (await factory.GetProjectsClientAsync())
                .GetAccessibleProjectsAsync(new AcademyCloud.Identity.Protos.Projects.GetAccessibleProjectsRequest
                {

                });

            return new GetAccessibleProjectsResponse()
            {
                Projects = resp.Projects.Select(x => new Project()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Active = true,
                    Admins = x.Admins.Select(TransformUser),
                    Members = x.Members.Select(TransformUser),
                    Resources = DummyResources,
                    UserResources = x.Admins.Concat(x.Members).ToDictionary(user => user.Id, user => DummyResources)
                })
            };
        }

        [HttpGet("{projectId}/users")]
        public async Task<GetUsersOfProjectResponse> GetUsersOfProject([FromRoute] string projectId)
        {
            var resp = await (await factory.GetProjectsClientAsync())
                .GetUsersOfProjectAsync(new AcademyCloud.Identity.Protos.Projects.GetUsersOfProjectRequest
                {
                    ProjectId = projectId
                });

            return new GetUsersOfProjectResponse
            {
                Admins = resp.Admins.Select(TransformUser),
                Members = resp.Members.Select(TransformUser),
                PayUser = TransformUser(resp.Admins[0]),
                UserResources = resp.Admins.Concat(resp.Members).ToDictionary(user => user.Id, user => DummyResources)
            };
        }

        [HttpPatch("{projectId}/users/{userId}/resources")]
        public async Task<ActionResult> SetResourcesOfUser([FromRoute] string projectId, [FromRoute] string userId, [FromBody] SetResourcesOfUserRequest request)
        {
            // request to expenses
            return NoContent();
        }

        [HttpPost("{projectId}/users")]
        public async Task<ActionResult> AddUserToProject([FromRoute] string projectId, [FromBody] AddUserToProjectRequest request)
        {

            var resp = await (await factory.GetProjectsClientAsync())
                .AddUserToProjectAsync(new AcademyCloud.Identity.Protos.Projects.AddUserToProjectRequest
                {
                    ProjectId = projectId,
                    UserId = request.UserId,
                    Role = (AcademyCloud.Identity.Protos.Common.UserRole)request.Role,
                });

            // add to expenses
            await (await factory.GetExpensesIdentityClient())
                .AddUserToProjectAsync(new AcademyCloud.Expenses.Protos.Identity.AddUserToProjectRequest
                {
                    ProjectId = projectId,
                    UserId = request.UserId,
                    UserProjectAssignmentId = resp.UserProjectAssignmentId,
                });

            return NoContent();
        }

        [HttpPatch("{projectId}/users/{userId}/role")]
        public async Task<ActionResult> ChangeUserRole([FromRoute] string projectId, [FromRoute] string userId, [FromBody] ChangeUserRoleRequest request)
        {
            var resp1 = await (await factory.GetProjectsClientAsync())
                .ChangeUserRoleAsync(new AcademyCloud.Identity.Protos.Projects.ChangeUserRoleRequest
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Role = (AcademyCloud.Identity.Protos.Common.UserRole)request.Role,
                });

            return NoContent();
        }

        [HttpDelete("{projectId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromProject([FromRoute] string projectId, [FromRoute] string userId)
        {
            // TODO request to expenses
            var resp1 = await (await factory.GetProjectsClientAsync())
                .RemoveUserFromProjectAsync(new AcademyCloud.Identity.Protos.Projects.RemoveUserFromProjectRequest
                {
                    ProjectId = projectId,
                    UserId = userId,
                });


            return NoContent();
        }

        [HttpPatch("{projectId}/resources")]
        public async Task<ActionResult> SetResources([FromRoute] string projectId)
        {
            // TODO request to expenses
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var res = await (await factory.GetProjectsClientAsync())
                .CreateProjectAsync(new AcademyCloud.Identity.Protos.Projects.CreateProjectRequest
                {
                    Name = request.Name,
                    AdminId = request.PayUserId,
                });

            // add project to expenses
            await (await factory.GetExpensesIdentityClient())
                .AddProjectAsync(new AcademyCloud.Expenses.Protos.Identity.AddProjectRequest
                {
                    Id = res.ProjectId,
                    PayUserAssignmentId = res.AdminAssignmentId,
                    PayUserId = request.PayUserId,
                });

            return Created(res.ProjectId, res.ProjectId);
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject([FromRoute] string projectId)
        {
            var resp = await (await factory.GetProjectsClientAsync())
                .DeleteProjectAsync(new AcademyCloud.Identity.Protos.Projects.DeleteProjectRequest
                {
                    ProjectId = projectId
                });

            return NoContent();
        }

        [HttpPatch("{projectId}/payUser")]
        public async Task<ActionResult> SetPayUser([FromRoute] string projectId, [FromBody] SetPayUserRequest request)
        {
            // request to expenses
            return NoContent();
        }




    }
}