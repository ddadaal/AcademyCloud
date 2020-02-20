using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Controllers.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public LoginResponse(string token)
        {
            Token = token;
        }
    }
}
