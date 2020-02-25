using AcademyCloud.Identity.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Test.DbContext
{
    public class MockDbContext
    {
        public static IEnumerable<User> Users { get; } = new List<User>()
        {
            new User(Guid.NewGuid(), "system1", "1", "system1@ac.com", true, new List<UserDomainAssignment>(), new List<UserProjectAssignment>()),
            new User(Guid.NewGuid(), "system2", "2", "system2@ac.com", true, new List<UserDomainAssignment>(), new List<UserProjectAssignment>()),
        };
    }
}
