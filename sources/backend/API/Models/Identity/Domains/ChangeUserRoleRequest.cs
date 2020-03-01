using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Domains
{
    public class ChangeUserRoleRequest
    {
        public UserRole Role { get; set; }
    }
}
