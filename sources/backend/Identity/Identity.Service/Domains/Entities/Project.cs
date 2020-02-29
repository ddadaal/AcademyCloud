using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domains.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Domain Domain { get; set; }

        public ICollection<UserProjectAssignment> Users { get; set; }

        public Project(Guid id, string name, Domain domain, ICollection<UserProjectAssignment> users)
        {
            Id = id;
            Name = name;
            Domain = domain;
            Users = users;
        }

        private Project() { }
    }
}
