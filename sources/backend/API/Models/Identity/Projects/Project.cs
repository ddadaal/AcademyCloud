using AcademyCloud.API.Models.Common;
using AcademyCloud.API.Models.Identity.Common;
using System.Collections.Generic;

namespace AcademyCloud.API.Models.Identity.Projects
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }

        public IEnumerable<User> Admins { get; set; }
        public IEnumerable<User> Members { get; set; }

        public User PayUser { get; set; }

        public Resources Quota { get; set; }

        public IDictionary<string, Resources> UserQuotas { get; set; }
    }
}