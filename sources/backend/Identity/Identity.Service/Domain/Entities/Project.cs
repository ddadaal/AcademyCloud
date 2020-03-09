using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual Domain Domain { get; set; }

        public virtual ICollection<UserProjectAssignment> Users { get; set; }

        public Project(Guid id, string name, Domain domain)
        {
            Id = id;
            Name = name;
            Domain = domain;
            Users = new List<UserProjectAssignment>();
        }

        protected Project() { }
    }
}
