using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Identity.Common;
using AcademyCloud.API.Models.Identity.Projects;
using AcademyCloud.API.Models.Identity.Users;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AcademyCloud.API.Utils.Dummies;

namespace AcademyCloud.API.Controllers.Identity
{
    [Route("/identity")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ServiceClientFactory factory;

        public UsersController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet("allUsers")]
        public async Task<ActionResult<GetAllUsersResponse>> GetAllUsers() 
        {
            var resp = await (await factory.GetUsersClientAsync())
                .GetAccessibleUsersAsync(new AcademyCloud.Identity.Protos.Users.GetAccessibleUsersRequest { });

            // request to expenses to get user activity

            // Pick ids and types
            var subjects = resp.Users.Select(x => new AcademyCloud.Expenses.Protos.Interop.Subject
            {
                Id = x.Id,
                Type = AcademyCloud.Expenses.Protos.Common.SubjectType.User,
            });

            // send the request
            var activitiesResp = await (await factory.GetExpensesInteropClientAsync())
                .GetActivityAsync(new AcademyCloud.Expenses.Protos.Interop.GetActivityRequest
                {
                    Subjects = { subjects }
                });

            // merge back activities
            var fullUsers = resp.Users.Select(x => new UserForSystem
            {
                Id = x.Id,
                Active = activitiesResp.Activities[x.Id],
                Name = x.Name,
            });

            return new GetAllUsersResponse 
            {
                Users = fullUsers,
            };
        }

        [HttpGet("users")]
        public async Task<ActionResult<GetAccessibleUsersResponse>> GetAccessibleUsers()
        {
            // Request to expenses
            var resp = await (await factory.GetUsersClientAsync())
                .GetAccessibleUsersAsync(new AcademyCloud.Identity.Protos.Users.GetAccessibleUsersRequest
                {

                });

            return new GetAccessibleUsersResponse
            {
                Users = resp.Users.Select(FromGrpc),
            };
        }

        [HttpDelete("users/{userId}")]
        public async Task<ActionResult> RemoveUserFromSystem([FromRoute] string userId)
        {
            // TODO request to expenses
            var resp = await (await factory.GetUsersClientAsync())
                .RemoveUserFromSystemAsync(new AcademyCloud.Identity.Protos.Users.RemoveUserFromSystemRequest
                {
                    UserId = userId
                });

            await (await factory.GetResourcesIdentityServiceClient())
                .DeleteUserAsync(new AcademyCloud.ResourceManagement.Protos.Identity.DeleteUserRequest
                {
                    UserId = userId
                });

            return NoContent();
        }
    }
}