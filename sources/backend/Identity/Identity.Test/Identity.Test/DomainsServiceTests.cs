﻿using AcademyCloud.Identity.Data;
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
        private IdentityDbContext db = default!;
        private DomainsService service = default!;

        [TestInitialize]
        public void Setup()
        {
            db = GetDbContext();
            service = new DomainsService(db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Database.EnsureDeleted();
            db.Dispose();
            service = null!;
        }

        [TestMethod]
        public async Task GetDomains()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var resp = await service.GetDomains(new GetDomainsRequest { }, context);

            Assert.AreEqual(3, resp.Domains.Count());
        }

        [TestMethod]
        public async Task GetUsersOfDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var resp = await service.GetUsersOfDomain(new GetUsersOfDomainRequest { DomainId = IdentityDbContext.SocialDomainId.ToString() }, context);

            Assert.AreEqual(1, resp.Admins.Count());
            Assert.AreEqual("Social Admin", resp.Admins[0].Name);
        }

        [TestMethod]
        public async Task AddUserToDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            Assert.AreEqual(2, db.Domains.Find(pku.Id).Users.Count);

            var resp = await service.AddUserToDomain(new AddUserToDomainRequest
            {
                DomainId = pku.Id.ToString(),
                UserId = cjy.Id.ToString(),
                Role = AcademyCloud.Identity.Services.Common.UserRole.Member
            }, context);

            Assert.AreEqual(3, db.Domains.Find(pku.Id).Users.Count);
            Assert.AreEqual(UserRole.Member, db.UserDomainAssignments.First(x => x.Domain.Id == pku.Id && x.User.Id == cjy.Id).Role);
        }

        [TestMethod]
        public async Task ChangeUserRole()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            Assert.AreEqual(UserRole.Member, nju.Users.First(x => x.User == cjd).Role);

            await service.ChangeUserRole(new ChangeUserRoleRequest
            {
                DomainId = nju.Id.ToString(),
                UserId = cjd.Id.ToString(),
                Role = AcademyCloud.Identity.Services.Common.UserRole.Admin
            }, context);

            Assert.AreEqual(UserRole.Admin, nju.Users.First(x => x.User == cjd).Role);
        }

        [TestMethod]
        public async Task CreateDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            Assert.AreEqual(3, db.Domains.Count());

            await service.CreateDomain(new CreateDomainRequest
            {
                Name = "testnewdomain",
                AdminId = cjd.Id.ToString(),
            }, context);

            Assert.AreEqual(4, db.Domains.Count());

            var domain = db.Domains.First(x => x.Name == "testnewdomain");
            Assert.AreEqual(1, domain.Users.Count());
            Assert.AreEqual(cjd.Id, domain.Users.First().User.Id);
        }

        [TestMethod]
        public async Task DeleteDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var prev =  db.Domains.Count();

            await service.DeleteDomain(new DeleteDomainRequest
            {
                DomainId = pku.Id.ToString(),
            }, context);

            Assert.AreEqual(prev - 1, db.Domains.Count());
            // should be cascade delete
            Assert.AreEqual(1, db.Projects.Count());
        }

        [TestMethod]
        public async Task RemoveUserFromDomain()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            var prev = cjd.Domains.Count();

            await service.RemoveUserFromDomain(new RemoveUserFromDomainRequest
            {
                DomainId = pku.Id.ToString(),
                UserId = cjd.Id.ToString()
            }, context);

            Assert.AreEqual(prev - 1, cjd.Domains.Count());
            Assert.AreEqual(nju.Name, cjd.Domains.First().Domain.Name);
        }

        [TestMethod]
        public async Task SetAdmins()
        {
            var context = await AuthenticatedCallContext.Create(db, "system", "system");

            await service.SetAdmins(new SetAdminsRequest
            {
                DomainId = nju.Id.ToString(),
                AdminIds = { lq.Id.ToString(), fc.Id.ToString() },
            }, context);

            Assert.AreEqual(2, db.Domains.Find(nju.Id).Users.Count(x => x.Role == UserRole.Admin));

            CollectionAssert.AreEqual(new List<Guid> { lq.Id, fc.Id }, db.Domains.Find(nju.Id).Users.Where(x => x.Role == UserRole.Admin).Select(x => x.User.Id).ToList());


            
        }
    }
}