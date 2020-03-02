using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services.Domains;
using Identity.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Identity.Test.Helpers.MockDbContext;
using System.Threading.Tasks;
using AcademyCloud.Identity.Domains.ValueObjects;

namespace Identity.Test
{
    [TestClass]
    public class DomainsServiceTests
    {
        private IdentityDbContext db;
        private DomainsService service;

        [TestInitialize]
        public void Setup()
        {
            db = MockDbContext.GetDbContext();
            service = new DomainsService(db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Database.EnsureDeleted();
            db.Dispose();
        }

        [TestMethod]
        public async Task TestGetDomains()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var resp = await service.GetDomains(new GetDomainsRequest { }, context);

            Assert.AreEqual(3, resp.Domains.Count());
        }

        [TestMethod]
        public async Task TestGetUsersOfDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var resp = await service.GetUsersOfDomain(new GetUsersOfDomainRequest { DomainId = IdentityDbContext.SocialDomainId.ToString() }, context);

            Assert.AreEqual(1, resp.Admins.Count());
            Assert.AreEqual("Social Admin", resp.Admins[0].Name);
        }

        [TestMethod]
        public async Task TestAddUserToDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var resp = await service.AddUserToDomain(new AddUserToDomainRequest
            {
                DomainId = pku.Id.ToString(),
                UserId = cjd.Id.ToString(),
                Role = AcademyCloud.Identity.Services.Common.UserRole.Member
            }, context);

            Assert.AreEqual(2, db.Domains.Find(pku.Id).Users.Count);
            Assert.AreEqual(UserRole.Member, db.UserDomainAssignments.First(x => x.Domain.Id == pku.Id && x.User.Id == cjd.Id).Role);

        }
    }
}
