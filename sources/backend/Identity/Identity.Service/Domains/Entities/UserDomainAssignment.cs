using AcademyCloud.Identity.Domains.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domains.Entities
{
    public class UserDomainAssignment
    {
        public Guid Id { get; set; }

        public virtual User User { get; set; }

        public virtual Domain Domain { get; set; }
        public UserRole Role { get; set; }

        public UserDomainAssignment(Guid id, User user, Domain domain, UserRole role)
        {
            Id = id;
            User = user;
            Domain = domain;
            Role = role;
        }

        public UserDomainAssignment() { }
    }
}
