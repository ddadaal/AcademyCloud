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

        public ICollection<UserDomainAssignment> Domains { get; set; }

        public ICollection<UserProjectAssignment> Projects { get; set; }

        public User(Guid id, string username, string password, string email, bool system, ICollection<UserDomainAssignment> domains, ICollection<UserProjectAssignment> projects)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            System = system;
            Domains = domains;
            Projects = projects;
        }

        private User()
        {
        }
    }
}
