using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services.Domains;
using Identity.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademyCloud.Identity.Domains.ValueObjects;
using Xunit;

namespace Identity.Test
{
    public class DomainsServiceTests : CommonTest
    {
        protected DomainsService service;

        public DomainsServiceTests()
        {
            service = new DomainsService(db);
        }

        [Fact]
        public async Task GetDomains()
        {

            var resp = await service.GetDomains(new GetDomainsRequest { }, TestContext);

            Assert.Equal(3, resp.Domains.Count());
        }

        [Fact]
        public async Task GetUsersOfDomain()
        {
            var resp = await service.GetUsersOfDomain(new GetUsersOfDomainRequest { DomainId = IdentityDbContext.SocialDomainId.ToString() }, TestContext);

            Assert.Single(resp.Admins);
            Assert.Equal("Social Admin", resp.Admins[0].Name);
        }

        [Fact]
        public async Task AddUserToDomain()
        {
            Assert.Equal(2, db.Domains.Find(pku.Id).Users.Count);

            var resp = await service.AddUserToDomain(new AddUserToDomainRequest
            {
                DomainId = pku.Id.ToString(),
                UserId = cjy.Id.ToString(),
                Role = AcademyCloud.Identity.Services.Common.UserRole.Member
            }, TestContext);

            Assert.Equal(3, db.Domains.Find(pku.Id).Users.Count);
            Assert.Equal(UserRole.Member, db.UserDomainAssignments.First(x => x.Domain.Id == pku.Id && x.User.Id == cjy.Id).Role);
        }

        [Fact]
        public async Task ChangeUserRole()
        {
            Assert.Equal(UserRole.Member, nju.Users.First(x => x.User == cjd).Role);

            await service.ChangeUserRole(new ChangeUserRoleRequest
            {
                DomainId = nju.Id.ToString(),
                UserId = cjd.Id.ToString(),
                Role = AcademyCloud.Identity.Services.Common.UserRole.Admin
            }, TestContext);

            Assert.Equal(UserRole.Admin, nju.Users.First(x => x.User == cjd).Role);
        }

        [Fact]
        public async Task CreateDomain()
        {

            Assert.Equal(3, db.Domains.Count());

            await service.CreateDomain(new CreateDomainRequest
            {
                Name = "testnewdomain",
                AdminId = cjd.Id.ToString(),
            }, TestContext);

            Assert.Equal(4, db.Domains.Count());

            var domain = db.Domains.First(x => x.Name == "testnewdomain");
            Assert.Single(domain.Users);
            Assert.Equal(cjd.Id, domain.Users.First().User.Id);
        }

        [Fact]
        public async Task DeleteDomain()
        {
            var prev = db.Domains.Count();

            await service.DeleteDomain(new DeleteDomainRequest
            {
                DomainId = pku.Id.ToString(),
            }, TestContext);

            Assert.Equal(prev - 1, db.Domains.Count());
            // should be cascade delete
            Assert.Equal(1, db.Projects.Count());
        }

        [Fact]
        public async Task RemoveUserFromDomain()
        {
            var prev = cjd.Domains.Count();

            await service.RemoveUserFromDomain(new RemoveUserFromDomainRequest
            {
                DomainId = pku.Id.ToString(),
                UserId = cjd.Id.ToString()
            },TestContext);

            Assert.Equal(prev - 1, cjd.Domains.Count());
            Assert.Equal(nju.Name, cjd.Domains.First().Domain.Name);
        }

        [Fact]
        public async Task SetAdmins()
        {
            await service.SetAdmins(new SetAdminsRequest
            {
                DomainId = nju.Id.ToString(),
                AdminIds = { lq.Id.ToString(), fc.Id.ToString() },
            }, TestContext);

            Assert.Equal(2, db.Domains.Find(nju.Id).Users.Count(x => x.Role == UserRole.Admin));

            Assert.Equal(new List<Guid> { lq.Id, fc.Id }, db.Domains.Find(nju.Id).Users.Where(x => x.Role == UserRole.Admin).Select(x => x.User.Id).ToList());



        }

    }
}
