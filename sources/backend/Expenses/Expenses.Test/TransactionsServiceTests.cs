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
            db.SaveChanges();

            var resp = await service.GetAccountTransactions(new Protos.Transactions.GetAccountTransactionsRequest(), TestContext);

            Assert.Single(resp.Transactions);
        }

        private void NjuPayManagementFee()
        {
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
        }

        [Fact]
        public async Task TestGetDomainTransactions()
        {
            service = new TransactionsService(MockTokenClaimsAccessor(njuadminnjuTokenClaims), db);

            NjuPayManagementFee();

            await db.SaveChangesAsync();

            var resp = await service.GetDomainTransactions(new Protos.Transactions.GetDomainTransactionsRequest()
            {
                DomainId = nju.Id.ToString(),
            }, TestContext);

            Assert.Single(resp.Transactions);
        }

        [Fact]
        public async Task TestGetProjectTransactions()
        {
            service = new TransactionsService(MockTokenClaimsAccessor(lqlqTokenClaims), db);

            lqproject.PayedOrgTransactions.Add(new Domain.Entities.OrgTransaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                -10,
                Domain.ValueObjects.TransactionReason.DomainManagement,
                lqproject,
                nju,
                new Domain.Entities.UserTransaction(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    -10,
                    Domain.ValueObjects.TransactionReason.DomainManagement,
                    lqproject.Payer,
                    nju.Payer
            )));

            await db.SaveChangesAsync();

            var resp = await service.GetProjectTransactions(new Protos.Transactions.GetProjectTransactionsRequest()
            {
                ProjectId = lqproject.Id.ToString(),
            }, TestContext);

            Assert.Single(resp.Transactions);
        }

        private void CjdPayManagementFee()
        {
            var system = db.Systems.First();

            cjd.PayedOrgTransactions.Add(new Domain.Entities.OrgTransaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                -10,
                Domain.ValueObjects.TransactionReason.UserManagement,
                cjd,
                system,
                new Domain.Entities.UserTransaction(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    -10,
                    Domain.ValueObjects.TransactionReason.UserManagement,
                    cjd,
                    system.SystemReceiver
                    )));
        }

        [Fact]
        public async Task TestGetSystemTransactions()
        {
            NjuPayManagementFee();
            CjdPayManagementFee();

            db.SaveChanges();

            service = new TransactionsService(MockTokenClaimsAccessor(njuadminnjuTokenClaims), db);

            var resp = await service.GetSystemTransactions(new Protos.Transactions.GetSystemTransactionsRequest { }, TestContext);

            Assert.Equal(2, resp.Transactions.Count);
        }

        [Fact]
        public async Task TestLimit()
        {
            NjuPayManagementFee();
            CjdPayManagementFee();

            db.SaveChanges();

            service = new TransactionsService(MockTokenClaimsAccessor(njuadminnjuTokenClaims), db);

            var resp = await service.GetSystemTransactions(new Protos.Transactions.GetSystemTransactionsRequest { }, TestContext);

            Assert.Equal(2, resp.Transactions.Count);

            resp = await service.GetSystemTransactions(new Protos.Transactions.GetSystemTransactionsRequest { Limit = 2 }, TestContext);
            Assert.Equal(2, resp.Transactions.Count);

            resp = await service.GetSystemTransactions(new Protos.Transactions.GetSystemTransactionsRequest { Limit = 1 }, TestContext);
            Assert.Equal(1, resp.Transactions.Count);

            resp = await service.GetSystemTransactions(new Protos.Transactions.GetSystemTransactionsRequest { Limit = -1 }, TestContext);
            Assert.Equal(2, resp.Transactions.Count);
        }


    }
}
