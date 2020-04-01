using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.Services.BillingCycle;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static AcademyCloud.Shared.Constants;
namespace AcademyCloud.Expenses.Test
{
    public class IdentityDomainServiceTests: CommonTest
    {
        private CombinedBillingCycleConfigurations billingConfiguration = new CombinedBillingCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };
        private CombinedUseCycleConfigurations useConfiguration = new CombinedUseCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };

        public (IdentityService, BillingCycleTask, UseCycleTask) CreateService(TokenClaims? tokenClaims = null)
        {
            var (billingTask, billingService) = ConfigureBillingCycleTask(billingConfiguration);
            var (useTask, useService) = ConfigureUseCycleTask(useConfiguration);
            var claimsAccessor = MockTokenClaimsAccessor(tokenClaims ?? njuadminnjuTokenClaims);

            return (new IdentityService(claimsAccessor, db, billingService), billingTask, useTask);
        }
        private Guid domainId = Guid.NewGuid();

        private IdentityService service;

        public IdentityDomainServiceTests()
        {
            var (service, _, _) = CreateService();
            this.service = service;
            service.AddDomain(new Protos.Identity.AddDomainRequest
            {
                Id = domainId.ToString(),
                PayUserId = cjd.Id.ToString(),
                PayUserAssignmentId = Guid.NewGuid().ToString(),
            }, TestContext).Wait();
        }

        [Fact]
        public async Task TestAddDomain()
        {

            var billingEntry = Assert.Single(db.BillingCycleEntries);
            Assert.Equal(domainId, billingEntry.Id);
            Assert.Single(db.Domains.Find(domainId).Users);
            Assert.Equal(cjd, billingEntry.Subject.PayUser);
            Assert.Single(cjd.Domains.Where(x => x.Domain.Id == domainId));
            Assert.NotNull(db.ManagementFeeEntries.FirstOrDefault(x => x.Id == domainId));
            Assert.NotNull(db.BillingCycleEntries.FirstOrDefault(x => x.Id == domainId));
            Assert.NotNull(db.UseCycleEntries.FirstOrDefault(x => x.Id == domainId));
        }


        [Fact]
        public async Task TestRemoveDomain()
        {

            await service.DeleteDomain(new Protos.Identity.DeleteDomainRequest
            {
                Id = domainId.ToString(),
            }, TestContext);

            Assert.Empty(db.BillingCycleEntries.Where(x => x.Subject.Domain.Id == domainId));
            Assert.Empty(cjd.Domains.Where(x => x.Domain.Id == domainId));
            Assert.Null(db.ManagementFeeEntries.FirstOrDefault(x => x.Id == domainId));
            Assert.Null(db.BillingCycleEntries.FirstOrDefault(x => x.Id == domainId));
            Assert.Null(db.UseCycleEntries.FirstOrDefault(x => x.Id == domainId));
        }

        [Fact]
        public async Task TestAddUserToDomain()
        {
            // Then add cjy into the domain
            await service.AddUserToDomain(new Protos.Identity.AddUserToDomainRequest
            {
                UserId = cjy.Id.ToString(),
                DomainId = domainId.ToString(),
                UserDomainAssignmentId = Guid.NewGuid().ToString(),
            }, TestContext);

            var domain = db.Domains.Find(domainId);
            Assert.Equal(new[] { cjd.Id, cjy.Id }.ToList(), domain.Users.Select(x => x.User.Id));
            Assert.Single(cjy.Domains.Where(x => x.Domain.Id == domainId));
        }

        [Fact]
        public async Task TestRemoveUserFromDomain()
        {


            // Then add cjy
            await service.AddUserToDomain(new Protos.Identity.AddUserToDomainRequest
            {
                UserId = cjy.Id.ToString(),
                DomainId = domainId.ToString(),
                UserDomainAssignmentId = Guid.NewGuid().ToString(),
            }, TestContext);

            // Then remove cjy
            await service.RemoveUserFromDomain(new Protos.Identity.RemoveUserFromDomainRequest
            {
                UserId = cjy.Id.ToString(),
                DomainId = domainId.ToString(),
            }, TestContext);

            Assert.Single(db.Domains.Find(domainId).Users);
            Assert.Empty(cjy.Domains);
        }

        [Fact]
        public async Task TestSetPayUser()
        {
            Assert.Equal(cjd, db.Domains.Find(domainId).PayUser);

            await service.SetDomainPayUser(new Protos.Identity.SetDomainPayUserRequest
            {
                DomainId = domainId.ToString(),
                PayUserId = cjy.Id.ToString(),
            }, TestContext);

            Assert.Equal(cjy, db.Domains.Find(domainId).PayUser);
        }

        [Fact]
        public async Task TestSetDomainQuota()
        {
            var domain = db.Domains.Find(domainId);
            Assert.Equal(Resources.Zero, domain.Quota);

            var newQuota = new Resources(10, 10, 10); 
            await service.SetDomainQuota(new Protos.Identity.SetDomainQuotaRequest
            {
                DomainId = domainId.ToString(),
                Quota = newQuota.ToGrpc(),
            }, TestContext);

            Assert.Equal(newQuota, domain.Quota);
            // Since previously it has zero quota, there should be no records
            Assert.Empty(domain.BillingCycleRecords);
            Assert.Empty(domain.PayedOrgTransactions);

            // change quota
            await service.SetDomainQuota(new Protos.Identity.SetDomainQuotaRequest
            {
                DomainId = domainId.ToString(),
                Quota = new Protos.Common.Resources { Cpu = 1, Memory = 1, Storage = 1 }
            }, TestContext);

            // Previous quota is not zero, there should be records
            var record = Assert.Single(domain.BillingCycleRecords);
            Assert.Equal(record.Quota, newQuota);
        }
    }
}
