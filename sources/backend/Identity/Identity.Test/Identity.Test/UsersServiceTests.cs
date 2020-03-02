using AcademyCloud.Identity.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static AcademyCloud.Identity.Test.Helpers.AuthenticatedCallContext;


namespace AcademyCloud.Identity.Test
{
    public class UsersServiceTests : CommonTest
    {
        [Fact]
        public async Task TestSystemGetUsers()
        {
            var service = new UsersService(db, await MockTokenClaimsAccessor(db));

            var resp = await service.GetAccessibleUsers(new GetAccessibleUsersRequest { }, TestContext);

            Assert.Equal(8, resp.Users.Count);


        }

        [Fact]
        public async Task TestDomainAdminGetUsers()
        {
            var service = new UsersService(db, await MockTokenClaimsAccessor(db, njuadmin));

            var resp = await service.GetAccessibleUsers(new GetAccessibleUsersRequest { }, TestContext);

            Assert.Equal(5, resp.Users.Count);
        }

        [Fact]
        public async Task TestProjectAdminGetUsers()
        {
            var service = new UsersService(db, await MockTokenClaimsAccessor(db, lq));

            var resp = await service.GetAccessibleUsers(new GetAccessibleUsersRequest { }, TestContext);

            Assert.Equal(2, resp.Users.Count);
        }

        [Fact]
        public async Task RemoveUserFromSystem()
        {
            var service = new UsersService(db, await MockTokenClaimsAccessor(db));

            await service.RemoveUserFromSystem(new RemoveUserFromSystemRequest
            {
                UserId = cjd.Id.ToString()
            }, TestContext);

            Assert.Equal(7, db.Users.Count());
        }

    }
}
