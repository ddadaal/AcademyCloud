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

        public static async Task FillDataAsync(IdentityDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            var nju = new Domain(Guid.NewGuid(), "NJU");
            var pku = new Domain(Guid.NewGuid(), "PKU");
            context.Domains.AddRange(nju, pku);

            var cjd = new User(Guid.NewGuid(), "cjd", "cjd", "cjd", "cjd@ac.com", false);
            var cjy = new User(Guid.NewGuid(), "cjy", "cjy", "cjy", "cjy@ac.com", false);
            var lq = new User(Guid.NewGuid(), "67", "67", "67", "67@ac.com", false);
            var fgh = new User(Guid.NewGuid(), "fgh", "fgh", "fgh", "fgh@ac.com", false);
            var fc = new User(Guid.NewGuid(), "fc", "fc", "fc", "fc@ac.com", false);
            var njuadmin = new User(Guid.NewGuid(), "njuadmin", "njuadmin", "njuadmin", "njuadmin@ac.com", false);
            context.Users.AddRange(cjd, cjy, lq, fgh, fc, njuadmin);

            var cjdnju = new UserDomainAssignment(Guid.NewGuid(), cjd, nju, UserRole.Member);
            var cjdpku = new UserDomainAssignment(Guid.NewGuid(), cjd, pku, UserRole.Member);
            var cjynju = new UserDomainAssignment(Guid.NewGuid(), cjy, nju, UserRole.Member);
            var lqnju = new UserDomainAssignment(Guid.NewGuid(), lq, nju, UserRole.Member);
            var fghnju = new UserDomainAssignment(Guid.NewGuid(), fgh, nju, UserRole.Member);
            var njuadminnju = new UserDomainAssignment(Guid.NewGuid(), njuadmin, nju, UserRole.Admin);
            var fcpku = new UserDomainAssignment(Guid.NewGuid(), fc, pku, UserRole.Admin);
            context.UserDomainAssignments.AddRange(cjdnju, cjdpku, cjynju, lqnju, fghnju, njuadminnju, fcpku);

            await context.SaveChangesAsync();
            

            

        }
    }
}
