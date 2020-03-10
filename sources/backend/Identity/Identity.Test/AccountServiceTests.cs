using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using AcademyCloud.Identity.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Account;
using AcademyCloud.Identity.Services;
using Xunit;
using static AcademyCloud.Identity.Test.Helpers.AuthenticatedCallContext;
using static AcademyCloud.Shared.Constants;

namespace AcademyCloud.Identity.Test
{
    public class AccountServiceTests : CommonTest
    {
        protected AccountService service;

        public AccountServiceTests()
        {
            // run all tests as njuadmin
            service = new AccountService(db, new Shared.JwtSettings(), MockTokenClaimsAccessor(db, fc).Result);
        }

        private async Task LoginAs(User user)
        {
            service = new AccountService(db, new Shared.JwtSettings(), await MockTokenClaimsAccessor(db, user));
        }

        [Fact]
        public async Task GetScopes()
        {
            var resp = await service.GetScopes(new GetScopesRequest
            {

            }, TestContext);

            Assert.Equal(2, resp.Scopes.Count);
        }

        [Fact]
        public async Task ExitDomain()
        {
            await LoginAs(cjd);
            await service.ExitDomain(new ExitDomainRequest
            {
                DomainId = nju.Id.ToString(),
            }, TestContext);

            Assert.Equal(1, cjd.Domains.Count);
            Assert.Equal(4, nju.Users.Count);
            Assert.Equal(1, lqproject.Users.Count);
        }

        [Fact]
        public async Task GetJoinableDomains()
        {
            var resp = await service.GetJoinableDomains(new GetJoinableDomainsRequest { }, TestContext);

            Assert.Equal(2, resp.Domains.Count());
        }

        [Fact]
        public async Task GetJoinedableDomains()
        {
            await LoginAs(cjd);
            var resp = await service.GetJoinedDomains(new GetJoinedDomainsRequest { }, TestContext);

            Assert.Equal(2, resp.Domains.Count());
        }

        [Fact]
        public async Task GetProfile()
        {
            await LoginAs(cjd);
            var resp = await service.GetProfile(new GetProfileRequest { }, TestContext);

            var profile = resp.Profile;

            Assert.Equal(cjd.Name, profile.Name);
            Assert.Equal(cjd.Username, profile.Username);
            Assert.Equal(cjd.Email, profile.Email);
            Assert.Equal(cjd.Id.ToString(), profile.Id);
        }

        [Fact]
        public async Task JoinDomain()
        {
            var resp = await service.JoinDomain(new JoinDomainRequest { DomainId = nju.Id.ToString() }, TestContext);

            Assert.Equal(2, fc.Domains.Count());
            Assert.Equal(UserRole.Member, fc.Domains.First(x => x.Domain == nju).Role);
        }

        [Fact]
        public async Task Register()
        {
            var resp = await service.Register(new RegisterRequest
            {
                Username = "test",
                Password = "test",
                Email = "test"
            }, TestContext);

            var user = db.Users.First(x => x.Username == "test");
            Assert.Equal(9, db.Users.Count());

            var socialDomain = db.Domains.Find(SocialDomainId);
            Assert.Equal(2, socialDomain.Users.Count());
            Assert.Single(socialDomain.Projects);

            var testProject = db.Projects.First(x => x.Domain == socialDomain && x.Name == "test");
            Assert.Single(testProject.Users);
            Assert.Equal(user, testProject.Users.First().User);

            Assert.Single(user.Projects);
            Assert.Equal(UserRole.Admin, user.Projects.First().Role);
        }

        [Fact]
        public async Task UpdatePasswordSuccess()
        {
            await LoginAs(cjd);
            var resp = await service.UpdatePassword(new UpdatePasswordRequest
            {
                Original = "cjd",
                Updated = "cjd1",
            }, TestContext);

            Assert.Equal(UpdatePasswordResponse.Types.Result.Success, resp.Result);
            Assert.Equal("cjd1", cjd.Password);
        }

        [Fact]
        public async Task UpdatePasswordFailed()
        {
            await LoginAs(cjd);
            var resp = await service.UpdatePassword(new UpdatePasswordRequest
            {
                Original = "cjd1",
                Updated = "cjd2",
            }, TestContext);

            Assert.Equal(UpdatePasswordResponse.Types.Result.OriginalNotMatch, resp.Result);
            Assert.Equal("cjd", cjd.Password);
        }

        [Fact]
        public async Task UpdateProfile()
        {
            await LoginAs(cjd);
            await service.UpdateProfile(new UpdateProfileRequest
            {
                Email = "1@1.com",
            }, TestContext);

            Assert.Equal("1@1.com", cjd.Email);
        }

    }
}
