using AcademyCloud.Identity.Services.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Identity.Test.Helpers.AuthenticatedCallContext;

namespace Identity.Test
{
    public class UsersServiceTests: CommonTest
    {
        [Fact]
        public async Task TestSystemGetUsers()
        {
            var context = await GetContext("system", "system");

            var service = new UsersService(db, await MockTokenClaimsAccessor(db, "system", "system"));

            var resp = await service.GetAccessibleUsers(new GetAccessibleUsersRequest { }, context);

            Assert.Equal(8, resp.Users.Count);


        }
    }
}
