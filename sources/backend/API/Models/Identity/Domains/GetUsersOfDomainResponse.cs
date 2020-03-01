using AcademyCloud.API.Models.Identity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Domains
{
    public class GetUsersOfDomainResponse
    {
        public IEnumerable<User> Admins { get; set; }

        public IEnumerable<User> Members { get; set; }

        public User PayUser { get; set; }
    }
}
