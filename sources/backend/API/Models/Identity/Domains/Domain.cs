using AcademyCloud.API.Models.Common;
using AcademyCloud.API.Models.Identity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Domains
{
    public class Domain
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }

        public IEnumerable<User> Admins { get; set; }

        public User PayUser { get; set; }

        public Resources Resources { get; set; }
    }
}
