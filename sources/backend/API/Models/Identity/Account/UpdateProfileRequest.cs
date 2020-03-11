using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class UpdateProfileRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
