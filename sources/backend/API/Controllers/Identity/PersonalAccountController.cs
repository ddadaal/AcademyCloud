using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Account;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.API.Controllers.Identity
{
    [Route("/identity/account")]
    [ApiController]
    public class PersonalAccountController : ControllerBase
    {
        private readonly ServiceClientFactory factory;

        public PersonalAccountController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpPost]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            var service = await factory.GetAccountClientAsync();

            var resp = await service.RegisterAsync(new AcademyCloud.Identity.Services.RegisterRequest()
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
            });

            return new RegisterResponse() { Scope = resp.Scope, Token = resp.Token };

        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ProfileResponse>> GetProfile()
        {
            var metadata = new Grpc.Core.Metadata
            {
                { "Authorization", "Bearer " + await HttpContext.GetTokenAsync("access_token") }
            };

            var service = await factory.GetAccountClientAsync();

            var resp = await service.GetProfileAsync(new AcademyCloud.Identity.Services.GetProfileRequest()
            {

            }, metadata);

            return new ProfileResponse() { Profile = resp.Profile };

        }

        [HttpPatch("profile")]
        [Authorize]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var service = await factory.GetAccountClientAsync();

            var resp = await service.UpdateProfileAsync(new AcademyCloud.Identity.Services.UpdateProfileRequest()
            {
                Email = request.Email,
            });

            return NoContent();
        }

        [HttpPatch("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            throw new NotImplementedException();

        }

        [HttpGet("joinedDomains")]
        [Authorize]
        public async Task<IActionResult> GetJoinedDomains()
        {

            throw new NotImplementedException();
        }

        [HttpDelete("joinedDomains/{domainId}")]
        [Authorize]
        public async Task<IActionResult> ExitDomain([FromRoute] string domainId)
        {
            throw new NotImplementedException();

        }


    }
}