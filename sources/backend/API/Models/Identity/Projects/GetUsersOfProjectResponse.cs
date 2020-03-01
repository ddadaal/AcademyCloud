using AcademyCloud.API.Models.Common;
using AcademyCloud.API.Models.Identity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Projects
{
    public class GetUsersOfProjectResponse
    {

        public IEnumerable<User> Admins { get; set; }

        public IEnumerable<User> Members { get; set; }

        public User PayUser { get; set; }

        public IDictionary<string, Resources> UserResources { get; set; }
    }
}
