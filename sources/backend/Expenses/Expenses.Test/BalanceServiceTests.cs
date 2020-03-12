using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
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

        [Fact]
        public async Task TestCharge()
        {
            var service = new BalanceService(MockTokenClaimsAccessor(cjdlqTokenClaims), db);

            var resp = await service.GetBalance(new AcademyCloud.Expenses.Protos.Balance.GetBalanceRequest { }, TestContext);

            Assert.Equal(10, resp.Balance);

            var resp2 = await service.Charge(new AcademyCloud.Expenses.Protos.Balance.ChargeRequest { Amount = 30 }, TestContext);

            Assert.Equal(40, resp2.Balance);
            Assert.Single(cjd.ReceivedUserTransactions);
            var transaction = cjd.ReceivedUserTransactions.First();
            Assert.Equal(30, transaction.Amount);
            Assert.Equal(cjd, transaction.Receiver);
            Assert.Null(transaction.Payer);
            Assert.Equal(new TransactionReason(TransactionType.Charge, ""), transaction.Reason);

        }
    }
}
