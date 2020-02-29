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

        public virtual ICollection<UserDomainAssignment> Users { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public Domain(Guid id, string name, ICollection<UserDomainAssignment> users = null, ICollection<Project> projects = null)
        {
            Id = id;
            Name = name;
            Users = users ?? new List<UserDomainAssignment>();
            Projects = projects ?? new List<Project>();
        }

        public Domain() { }
    }
}
