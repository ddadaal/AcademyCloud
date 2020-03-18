using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public bool UserActive { get; set; }

        public bool ScopeActive { get; set; }
        public string Token { get; set; }
    }
}
