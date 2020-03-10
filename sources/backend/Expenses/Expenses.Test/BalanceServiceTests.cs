using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.Test
{
    public class BalanceServiceTests : CommonTest
    {
        [Fact]
        public async Task TestGetBalance()
        {
            var service = new BalanceService(MockTokenClaimsAccessor(cjdlqTokenClaims), db);

            var resp = await service.GetBalance(new AcademyCloud.Expenses.Protos.Balance.GetBalanceRequest { }, TestContext);

            Assert.Equal(10, resp.Balance);
        }
    }
}
