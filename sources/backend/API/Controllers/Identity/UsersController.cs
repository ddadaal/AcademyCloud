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
    [Route("/identity/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ServiceClientFactory factory;

        public UsersController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public async Task<ActionResult<GetAccessibleUsersResponse>> GetAccessibleUsers()
        {
            // Request to expenses
            var resp = await (await factory.GetUsersClientAsync())
                .GetAccessibleUsersAsync(new AcademyCloud.Identity.Services.Users.GetAccessibleUsersRequest
                {

                });

            return new GetAccessibleUsersResponse
            {
                Users = resp.Users.Select(TransformUser),
            };
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> RemoveUserFromSystem([FromRoute] string userId)
        {
            // TODO request to expenses
            var resp = await (await factory.GetUsersClientAsync())
                .RemoveUserFromSystemAsync(new AcademyCloud.Identity.Services.Users.RemoveUserFromSystemRequest
                {
                    UserId = userId
                });

            return NoContent();
        }
    }
}