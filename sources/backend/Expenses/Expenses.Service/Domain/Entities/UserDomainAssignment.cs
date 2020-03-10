using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class UserDomainAssignment
    {
        public Guid Id { get; set; }

        public virtual Domain Domain { get; set; }

        public virtual User User { get; set; }

        public UserDomainAssignment(Guid id, Domain domain, User user)
        {
            Id = id;
            Domain = domain;
            User = user;
        }

        protected UserDomainAssignment()
        {
        }
    }
}
