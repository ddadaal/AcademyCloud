using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using AcademyCloud.Identity.Domains.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Test.Helpers
{
    public class MockDbContext
    {
        public static Domain nju = new Domain(Guid.NewGuid(), "NJU");
        public static Domain pku = new Domain(Guid.NewGuid(), "PKU");
        public static User cjd = new User(Guid.NewGuid(), "cjd", "cjd", "cjd", "cjd@ac.com", false);
        public static User cjy = new User(Guid.NewGuid(), "cjy", "cjy", "cjy", "cjy@ac.com", false);
        public static User lq = new User(Guid.NewGuid(), "67", "67", "67", "67@ac.com", false);
        public static User fgh = new User(Guid.NewGuid(), "fgh", "fgh", "fgh", "fgh@ac.com", false);
        public static User fc = new User(Guid.NewGuid(), "fc", "fc", "fc", "fc@ac.com", false);
        public static User njuadmin = new User(Guid.NewGuid(), "njuadmin", "njuadmin", "njuadmin", "njuadmin@ac.com", false);
        public static UserDomainAssignment cjdnju = new UserDomainAssignment(Guid.NewGuid(), cjd, nju, UserRole.Member);
        public static UserDomainAssignment cjynju = new UserDomainAssignment(Guid.NewGuid(), cjy, nju, UserRole.Member);
        public static UserDomainAssignment lqnju = new UserDomainAssignment(Guid.NewGuid(), lq, nju, UserRole.Member);
        public static UserDomainAssignment fghnju = new UserDomainAssignment(Guid.NewGuid(), fgh, nju, UserRole.Member);
        public static UserDomainAssignment njuadminnju = new UserDomainAssignment(Guid.NewGuid(), njuadmin, nju, UserRole.Admin);
        public static UserDomainAssignment fcpku = new UserDomainAssignment(Guid.NewGuid(), fc, pku, UserRole.Admin);
        public static Project lqproject = new Project(Guid.NewGuid(), "67 Project", nju);
        public static Project fcproject = new Project(Guid.NewGuid(), "fc Project", pku);
        public static UserProjectAssignment lq67project = new UserProjectAssignment(Guid.NewGuid(), lq, lqproject, UserRole.Admin);
        public static UserProjectAssignment cjd67project = new UserProjectAssignment(Guid.NewGuid(), cjd, lqproject, UserRole.Member);

        public static void FillData(IdentityDbContext context)
        {
            // system user: system, system

            context.Database.EnsureCreated();

            context.Domains.AddRange(nju, pku);

            context.Users.AddRange(cjd, cjy, lq, fgh, fc, njuadmin);

            context.UserDomainAssignments.AddRange(cjdnju, cjynju, lqnju, fghnju, njuadminnju, fcpku);

            context.Projects.AddRange(lqproject, fcproject);

            context.UserProjectAssignments.AddRange(lq67project, cjd67project);

            context.SaveChanges();

        }

        public static IdentityDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

            var context = new IdentityDbContext(options);

            FillData(context);

            return context;
        }
    }
}
