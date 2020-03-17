using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Common;
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

            var subjects = resp.Projects
                .Select(x => new AcademyCloud.Expenses.Protos.Interop.Subject
                {
                    Id = x.Id,
                    Type = AcademyCloud.Expenses.Protos.Common.SubjectType.Project,
                });


            // get pay users
            var payUsers = await (await factory.GetExpensesInteropClientAsync())
                .GetPayUserAsync(new AcademyCloud.Expenses.Protos.Interop.GetPayUserRequest
                {
                    Subjects = { subjects }
                });

            // get pay user's names
            var payUserNames = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new AcademyCloud.Identity.Protos.Interop.GetNamesRequest
                {
                    Subjects =
                    {
                        payUsers.PayUsers.Values.Select(x => new AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.Subject
                        {
                            Id = x,
                            Type = AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.SubjectType.User,
                        })
                    }
                });

            var quotaSubjects = subjects
                .Concat(
                resp.Projects
                    .Select(x => x.Admins)
                    .Concat(resp.Projects.Select(x => x.Members))
                        .SelectMany(x => x)
                        .Select(x => new AcademyCloud.Expenses.Protos.Interop.Subject
                        {
                            Id = x.UserProjectAssignmentId.ToString(),
                            Type = AcademyCloud.Expenses.Protos.Common.SubjectType.UserProjectAssignment
                        }
                ));

            // get quotas
            var quotas = await (await factory.GetExpensesInteropClientAsync())
                .GetQuotaAsync(new AcademyCloud.Expenses.Protos.Interop.GetQuotaRequest
                {
                    Subjects = { quotaSubjects }
                });


            return new GetAccessibleProjectsResponse()
            {
                Projects = resp.Projects.Select(x => new Project()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Active = true,
                    Admins = x.Admins.Select(x => FromGrpc(x.User)),
                    Members = x.Members.Select(x => FromGrpc(x.User)),
                    Quota = quotas.Quotas[x.Id],
                    UserQuotas = x.Admins.Concat(x.Members).ToDictionary(user => user.User.Id, user => (Resources)quotas.Quotas[user.UserProjectAssignmentId])
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

            // get quotas from expenses
            var subjects = resp.Admins.Concat(resp.Members)
                .Select(x => new AcademyCloud.Expenses.Protos.Interop.Subject
                {
                    Id = x.UserProjectAssignmentId.ToString(),
                    Type = AcademyCloud.Expenses.Protos.Common.SubjectType.UserProjectAssignment
                });

            var quotas = await (await factory.GetExpensesInteropClientAsync())
                .GetQuotaAsync(new AcademyCloud.Expenses.Protos.Interop.GetQuotaRequest
                {
                    Subjects = { subjects }
                });

            return new GetUsersOfProjectResponse
            {
                Admins = resp.Admins.Select(x => FromGrpc(x.User)),
                Members = resp.Members.Select(x => FromGrpc(x.User)),
                UserResources = resp.Admins.Concat(resp.Members).ToDictionary(user => user.User.Id, user => (Resources)quotas.Quotas[user.UserProjectAssignmentId])
            };
        }

        [HttpPatch("{projectId}/users/{userId}/resources")]
        public async Task<ActionResult> SetResourcesOfUser([FromRoute] string projectId, [FromRoute] string userId, [FromBody] SetResourcesOfUserRequest request)
        {
            // request to expenses
            await (await factory.GetExpensesIdentityClient())
                .SetProjectUserQuotaAsync(new AcademyCloud.Expenses.Protos.Identity.SetProjectUserQuotaRequest
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Quota = request.Resources,
                });

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

            // add to resources
            await (await factory.GetResourcesIdentityServiceClient())
                .AddUserAsync(new AcademyCloud.ResourceManagement.Protos.Identity.AddUserRequest
                {
                    UserId = request.UserId,
                    ProjectId = projectId,
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
            var resp1 = await (await factory.GetProjectsClientAsync())
                .RemoveUserFromProjectAsync(new AcademyCloud.Identity.Protos.Projects.RemoveUserFromProjectRequest
                {
                    ProjectId = projectId,
                    UserId = userId,
                });

            // request to expenses
            await (await factory.GetExpensesIdentityClient())
                .RemoveUserFromProjectAsync(new AcademyCloud.Expenses.Protos.Identity.RemoveUserFromProjectRequest
                {
                    UserId = userId,
                    ProjectId = projectId,
                });


            // request to resources
            await (await factory.GetResourcesIdentityServiceClient())
                .DeleteUserAsync(new AcademyCloud.ResourceManagement.Protos.Identity.DeleteUserRequest
                {
                    UserId = userId,
                    ProjectId = projectId
                });

            return NoContent();
        }

        [HttpPatch("{projectId}/resources")]
        public async Task<ActionResult> SetResources([FromRoute] string projectId, [FromBody] SetResourcesRequest request)
        {
            // request to expenses
            await (await factory.GetExpensesIdentityClient())
                .SetProjectQuotaAsync(new AcademyCloud.Expenses.Protos.Identity.SetProjectQuotaRequest
                {
                    ProjectId = projectId,
                    Quota = request.Resources,
                });

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

            await (await factory.GetExpensesIdentityClient())
                .DeleteProjectAsync(new AcademyCloud.Expenses.Protos.Identity.DeleteProjectRequest
                {
                    Id = projectId
                });

            return NoContent();
        }

        [HttpPatch("{projectId}/payUser")]
        public async Task<ActionResult> SetPayUser([FromRoute] string projectId, [FromBody] SetPayUserRequest request)
        {
            // request to expenses
            await (await factory.GetExpensesIdentityClient())
                .SetProjectPayUserAsync(new AcademyCloud.Expenses.Protos.Identity.SetProjectPayUserRequest
                {
                    ProjectId = projectId,
                    PayUserId = request.PayUserId,
                });

            return NoContent();
        }

    }
}