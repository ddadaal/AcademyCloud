using System;
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
            var scope = request.Scope;

            var reply = await (await factory.GetAuthenticationClientAsync())
                .AuthenticateAsync(new AuthenticationRequest()
                {
                    Username = request.Username,
                    Password = request.Password,
                    Scope = scope,
                });

            var (userActive, scopeActive) = await GetUserAndScopeActivity(reply.UserId, scope);


            if (reply.Success)
            {
                return new LoginResponse
                {
                    UserId = reply.UserId,
                    Token = reply.Token,
                    UserActive = userActive,
                    ScopeActive = scopeActive,
                };
            }
            else
            {
                return StatusCode(403);
            }
        }

        private async Task<(bool, bool)> GetUserAndScopeActivity(string userId, Scope scope)
        {
            (AcademyCloud.Expenses.Protos.Common.SubjectType, string) subject;
            if (scope.System)
            {
                subject = (AcademyCloud.Expenses.Protos.Common.SubjectType.System, "");
            }
            else if (string.IsNullOrEmpty(scope.ProjectId))
            {
                subject = (AcademyCloud.Expenses.Protos.Common.SubjectType.Domain, scope.DomainId);
            }
            else
            {
                subject = (AcademyCloud.Expenses.Protos.Common.SubjectType.Project, scope.ProjectId);
            }

            // get activity
            var activityResp = await (await factory.GetExpensesInteropClientAsync())
                .GetActivityAsync(new AcademyCloud.Expenses.Protos.Interop.GetActivityRequest
                {
                    Subjects =
                    {
                        new AcademyCloud.Expenses.Protos.Interop.Subject { Type = AcademyCloud.Expenses.Protos.Common.SubjectType.User, Id = userId },
                        new AcademyCloud.Expenses.Protos.Interop.Subject
                        {
                            Type = subject.Item1,
                            Id = subject.Item2,
                        }
                    }
                });

            return (activityResp.Activities[userId], activityResp.Activities[subject.Item2]);
        }



        [HttpPost("token/change")]
        public async Task<ActionResult<LoginResponse>> ChangeScope([FromBody] Models.ChangeScopeRequest request)
        {
            var reply = await (await factory.GetAuthenticationClientAsync())
                .ChangeScopeAsync(new AcademyCloud.Identity.Protos.Authentication.ChangeScopeRequest
                {
                    Scope = request.Scope,
                });

            var (userActive, scopeActive) = await GetUserAndScopeActivity(reply.UserId, request.Scope);

            if (reply.Success)
            {
                return new LoginResponse
                {
                    UserId = reply.UserId,
                    Token = reply.Token,
                    ScopeActive = scopeActive,
                    UserActive = userActive,
                };
            }
            else
            {
                return StatusCode(403);
            }

        }
    }
}