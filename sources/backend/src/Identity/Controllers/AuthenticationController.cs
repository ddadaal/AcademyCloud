using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.Identity.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IdentityDbContext dbContext;

        public AuthenticationController(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("scopes")]
        public async Task<ActionResult<ScopesResponse>> GetScopes([FromQuery]string username, [FromQuery] string password)
        {
            // find the user first
            var user = dbContext.Users.First(user => user.Username == username && user.Password == password);

            if (user == null)
            {
                return 
            }

          
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
