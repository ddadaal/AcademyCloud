using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domains.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }

        public bool System { get; set; }

        public virtual ICollection<UserDomainAssignment> Domains { get; set; }

        public virtual ICollection<UserProjectAssignment> Projects { get; set; }

        public User(Guid id, string username, string password, string email, bool system,
            ICollection<UserDomainAssignment> domains = null,
            ICollection<UserProjectAssignment> projects = null)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            System = system;
            Domains = domains ?? new List<UserDomainAssignment>();
            Projects = projects ?? new List<UserProjectAssignment>();
        }

        public User()
        {
        }
    }
}
