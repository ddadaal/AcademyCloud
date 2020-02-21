using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domains.Entities
{
    public class Domain
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserDomainAssignment> Users { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
