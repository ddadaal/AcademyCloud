using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using AcademyCloud.Identity.Domains.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Test.DbContext
{
    public class MockDbContext
    {
        public static IEnumerable<User> Users { get; } = new List<User>()
        {
            new User(Guid.NewGuid(), "system2", "2", "system2@ac.com", true, new List<UserDomainAssignment>(), new List<UserProjectAssignment>()),
        };

        public static async Task FillDataAsync(IdentityDbContext context)
        {
            var system1 = new User(Guid.NewGuid(), "system1", "1", "system1@ac.com", true);
            var system2 = new User(Guid.NewGuid(), "system2", "2", "system2@ac.com", true);

            var nju = new Domain(Guid.NewGuid(), "NJU");
            var pku = new Domain(Guid.NewGuid(), "PKU");

            var cjd = new User(Guid.NewGuid(), "cjd", "cjd", "cjd@ac.com", false);
            var cjy = new User(Guid.NewGuid(), "cjy", "cjy", "cjy@ac.com", false);
            var lq = new User(Guid.NewGuid(), "67", "67", "67@ac.com", false);
            var fgh = new User(Guid.NewGuid(), "fgh", "fgh", "fgh@ac.com", false);
            var fc = new User(Guid.NewGuid(), "fc", "fc", "fc@ac.com", false);
            var njuadmin = new User(Guid.NewGuid(), "njuadmin", "njuadmin", "njuadmin@ac.com", false);

            var cjdnju = new UserDomainAssignment(Guid.NewGuid(), cjd, nju, UserRole.Member, DateTime.Now);
            var cjdpku = new UserDomainAssignment(Guid.NewGuid(), cjd, pku, UserRole.Member, DateTime.Now);
            var cjynju = new UserDomainAssignment(Guid.NewGuid(), cjy, nju, UserRole.Member, DateTime.Now);
            var lqnju = new UserDomainAssignment(Guid.NewGuid(), lq, nju, UserRole.Member, DateTime.Now);
            var fghnju = new UserDomainAssignment(Guid.NewGuid(), fgh, nju, UserRole.Member, DateTime.Now);
            var njuadminnju = new UserDomainAssignment(Guid.NewGuid(), njuadmin, nju, UserRole.Admin, DateTime.Now);
            var fcpku = new UserDomainAssignment(Guid.NewGuid(), fc, pku, UserRole.Admin, DateTime.Now);

            

        }
    }
}
