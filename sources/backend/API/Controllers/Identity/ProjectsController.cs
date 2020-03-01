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
            var client = await factory.GetProjectsClientAsync();

            var resp = await client.GetAccessibleProjectsAsync(new AcademyCloud.Identity.Services.Projects.GetAccessibleProjectsRequest
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
            var client = await factory.GetProjectsClientAsync();

            var resp = await client.GetUsersOfProjectAsync(new AcademyCloud.Identity.Services.Projects.GetUsersOfProjectRequest
            {
                ProjectId = projectId
            });

            return new GetUsersOfProjectResponse
            {
                Admins = resp.Admins.Select(TransformUser),
                Members = resp.Members.Select(TransformUser),
                PayUser = TransformUser(resp.PayUser),
                UserResources = resp.Admins.Concat(resp.Members).ToDictionary(user => user.Id, user => DummyResources)
            };
        }

        [HttpPatch("{projectId}/users/{userId}/resources")]
        public async Task<ActionResult> SetResourcesOfUser([FromRoute] string projectId, [FromRoute] string userId, [FromBody] SetResourcesOfUserRequest request)
        {
            // request to expenses
            return NoContent();
        }

        [HttpPost("{projectId}/users/{userId}")]
        public async Task<ActionResult> AddUserToProject([FromRoute] string projectId, [FromBody] AddUserToProjectRequest request)
        {
            var client = await factory.GetProjectsClientAsync();

            // TODO request to expenses
            var resp = await client.AddUserToProjectAsync(new AcademyCloud.Identity.Services.Projects.AddUserToProjectRequest
            {
                ProjectId = projectId,
                UserId = request.UserId,
                Role = (AcademyCloud.Identity.Services.Common.UserRole)request.Role,
            });

            return NoContent();
        }

        [HttpPatch("{projectId}/users/{userId}/role")]
        public async Task<ActionResult> ChangeUserRole([FromRoute] string projectId, [FromRoute] string userId, [FromBody] ChangeUserRoleRequest request)
        {

            var client = await factory.GetProjectsClientAsync();

            var resp = await client.ChangeUserRoleAsync(new AcademyCloud.Identity.Services.Projects.ChangeUserRoleRequest
            {
                ProjectId = projectId,
                UserId = userId,
                Role = (AcademyCloud.Identity.Services.Common.UserRole)request.Role,
            });
            return NoContent();
        }

        [HttpDelete("{projectId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromProject([FromRoute] string projectId, [FromRoute] string userId)
        {
            var client = await factory.GetProjectsClientAsync();
            
            // TODO request to expenses
            await client.RemoveUserFromProjectAsync(new AcademyCloud.Identity.Services.Projects.RemoveUserFromProjectRequest
            {
                ProjectId = projectId,
                UserId = userId,
            });

            return NoContent();
        }


    }
}