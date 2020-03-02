using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domains.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }

        public bool System { get; set; }

        public virtual ICollection<UserDomainAssignment> Domains { get; set; }

        public virtual ICollection<UserProjectAssignment> Projects { get; set; }

        public User(Guid id, string username, string name, string password, string email, bool system)
        {
            Id = id;
            Username = username;
            Name = name;
            Password = password;
            Email = email;
            System = system;
            Domains = new List<UserDomainAssignment>();
            Projects = new List<UserProjectAssignment>();
        }

        protected User() { }
    }
}
