using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
{
    public class TransactionsServiceTests : CommonTest
    {
        protected TransactionsService service;

        public TransactionsServiceTests()
        {
            service = new TransactionsService(MockTokenClaimsAccessor(cjdlqTokenClaims), db);
        }

        [Fact]
        public async Task TestGetAccountTransactions()
        {
            cjd.ReceivedUserTransactions.Add(new Domain.Entities.UserTransaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                10,
                Domain.ValueObjects.TransactionReason.Charge,
                null,
                cjd
            ));

            await db.SaveChangesAsync();

            var resp = await service.GetAccountTransactions(new Protos.Transactions.GetAccountTransactionsRequest(), TestContext);

            Assert.Single(resp.Transactions);
        }

        [Fact]
        public async Task TestGetDomainTransactions()
        {
            service = new TransactionsService(MockTokenClaimsAccessor(njuadminnjuTokenClaims), db);
            var system = db.Systems.First();

            nju.PayedOrgTransactions.Add(new Domain.Entities.OrgTransaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                -10,
                Domain.ValueObjects.TransactionReason.DomainManagement,
                nju,
                system,
                new Domain.Entities.UserTransaction(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    -10,
                    Domain.ValueObjects.TransactionReason.DomainManagement,
                    nju.Payer,
                    system.SystemReceiver
            )));

            await db.SaveChangesAsync();

            var resp = await service.GetDomainTransactions(new Protos.Transactions.GetDomainTransactionsRequest()
            {
                DomainId = nju.Id.ToString(),
            }, TestContext);

            Assert.Single(resp.Transactions);
        }
    }
}
