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

        public User User { get; set; }

        public Domain Domain { get; set; }

        public DateTime AssignTime { get; set; }

        public UserRole Role { get; set; }
    }
}
