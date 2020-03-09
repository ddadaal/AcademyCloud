﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models;
using AcademyCloud.API.Utils;
using AcademyCloud.Identity.Protos;
using AcademyCloud.Identity.Protos.Authentication;
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



        [HttpGet("scopes")]
        public async Task<ActionResult<ScopesResponse>> GetScopes([FromQuery]string username = "", [FromQuery] string password = "")
        {
            var reply = await (await factory.GetAuthenticationClientAsync())
                .GetScopesAsync(new GetScopesRequest()
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

        [HttpPost("token")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var reply = await (await factory.GetAuthenticationClientAsync())
                .AuthenticateAsync(new AuthenticationRequest()
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

        [HttpPost("token/change")]
        public async Task<ActionResult<LoginResponse>> ChangeScope([FromBody] Models.ChangeScopeRequest request)
        {
            var reply = await (await factory.GetAuthenticationClientAsync())
                .ChangeScopeAsync(new AcademyCloud.Identity.Protos.Authentication.ChangeScopeRequest
                {
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