using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.API.Controllers.Authentication
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet]
        [Route("scopes")]
        public async Task<ActionResult<ScopesResponse>> GetScopes([FromQuery]string username, [FromQuery] string password)
        {
            // TODO
            return null;
        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            // TODO
            return null;
        }
    }
}