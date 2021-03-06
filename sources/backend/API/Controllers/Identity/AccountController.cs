﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models;
using AcademyCloud.API.Models.Identity.Account;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.API.Controllers.Identity
{
    [Route("/identity/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ServiceClientFactory factory;

        public AccountController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet("scopes")]
        [Authorize]
        public async Task<ActionResult<GetScopesResponse>> GetScopes()
        {
            var resp = await (await factory.GetAccountClientAsync())
                .GetScopesAsync(new AcademyCloud.Identity.Protos.Account.GetScopesRequest());

            return new GetScopesResponse { Scopes = resp.Scopes };
        }

        [HttpPost]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            var resp = await (await factory.GetAccountClientAsync())
                .RegisterAsync(new AcademyCloud.Identity.Protos.Account.RegisterRequest()
                {
                    Username = request.Username,
                    Password = request.Password,
                    Email = request.Email,
                });

            // add user to expenses
            await (await factory.GetExpensesIdentityClient())
                .AddUserAsync(new AcademyCloud.Expenses.Protos.Identity.AddUserRequest
                {
                    UserId = resp.UserId,
                    SocialProjectId = resp.SocialProjectId,
                    SocialDomainAssignmentId = resp.SocialDomainAssignmentId,
                    SocialProjectAssignmentId = resp.SocialProjectAssignmentId,
                });

            // add user to resources
            await (await factory.GetResourcesIdentityServiceClient())
                .AddUserAsync(new AcademyCloud.ResourceManagement.Protos.Identity.AddUserRequest
                {
                    UserId = resp.UserId,
                    ProjectId = resp.SocialProjectId,
                    UserProjectAssignmentId = resp.SocialProjectAssignmentId,
                }) ;

            return new RegisterResponse() { Scope = resp.Scope, Token = resp.Token };

        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ProfileResponse>> GetProfile()
        {
            var resp = await (await factory.GetAccountClientAsync())
                .GetProfileAsync(new AcademyCloud.Identity.Protos.Account.GetProfileRequest()
                {

                });

            return new ProfileResponse() { Profile = resp.Profile };

        }

        [HttpPatch("profile")]
        [Authorize]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var resp = await (await factory.GetAccountClientAsync())
                .UpdateProfileAsync(new AcademyCloud.Identity.Protos.Account.UpdateProfileRequest()
                {
                    Email = request.Email,
                    Name = request.Name,
                });

            return NoContent();
        }

        [HttpPatch("password")]
        [Authorize]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var resp = await (await factory.GetAccountClientAsync())
                .UpdatePasswordAsync(new AcademyCloud.Identity.Protos.Account.UpdatePasswordRequest()
                {
                    Original = request.Original,
                    Updated = request.Updated,
                });

            if (resp.Result == AcademyCloud.Identity.Protos.Account.UpdatePasswordResponse.Types.Result.Success)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(409);
            }
        }

        [HttpGet("joinedDomains")]
        [Authorize]
        public async Task<ActionResult<GetJoinedDomainsResponse>> GetJoinedDomains()
        {
            var resp = await (await factory.GetAccountClientAsync())
                .GetJoinedDomainsAsync(new AcademyCloud.Identity.Protos.Account.GetJoinedDomainsRequest()
                {

                });

            return new GetJoinedDomainsResponse() { Domains = resp.Domains };


        }

        [HttpDelete("joinedDomains/{domainId}")]
        [Authorize]
        public async Task<ActionResult> ExitDomain([FromRoute] string domainId)
        {
            var resp = await (await factory.GetAccountClientAsync())
                .ExitDomainAsync(new AcademyCloud.Identity.Protos.Account.ExitDomainRequest()
                {
                    DomainId = domainId
                });

            return NoContent();
        }

        [HttpGet("joinableDomains")]
        [Authorize]
        public async Task<ActionResult<GetJoinableDomainsResponse>> GetJoinableDomains()
        {
            var resp = await (await factory.GetAccountClientAsync())
                .GetJoinableDomainsAsync(new AcademyCloud.Identity.Protos.Account.GetJoinableDomainsRequest()
                {

                });

            return new GetJoinableDomainsResponse { Domains = resp.Domains };
        }
        [HttpPost("joinableDomains/{domainId}")]
        public async Task<ActionResult> JoinDomain([FromRoute] string domainId)
        {
            var resp = await (await factory.GetAccountClientAsync())
                .JoinDomainAsync(new AcademyCloud.Identity.Protos.Account.JoinDomainRequest()
                {
                    DomainId = domainId
                });

            return NoContent();

        }


    }
}