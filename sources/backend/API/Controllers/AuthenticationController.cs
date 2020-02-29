using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models;
using AcademyCloud.API.Utils;
using AcademyCloud.Identity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ServiceClientFactory factory;

        public AuthenticationController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        [Route("scopes")]
        public async Task<ActionResult<ScopesResponse>> GetScopes([FromQuery]string username, [FromQuery] string password)
        {
            var authClient = await factory.GetAuthenticationClientAsync();

            var reply = await authClient.GetScopesAsync(new GetScopesRequest()
            {
                Username = username,
                Password = password,
            });

            if (reply.Success)
            {
                return new ScopesResponse(reply.Scopes, reply.Scopes[0], reply.Scopes[0]);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var authClient = await factory.GetAuthenticationClientAsync();

            var reply = await authClient.AuthenticateAsync(new AuthenticationRequest()
            {
                Username = request.Username,
                Password = request.Password,
                Scope = request.Scope,
            });

            if (reply.Success)
            {
                return new LoginResponse(reply.Token);
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}