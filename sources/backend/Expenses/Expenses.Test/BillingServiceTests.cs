using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Billing;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
{
    public class BillingServiceTests : CommonTest
    {
        private BillingCycleConfigurations billingConfiguration = new BillingCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };
        private UseCycleConfigurations useConfiguration = new UseCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };

        public BillingService CreateService()
        {
            var billingTask = ConfigureTask<BillingCycleTask, BillingCycleConfigurations>(billingConfiguration);
            var useTask = ConfigureTask<UseCycleTask, UseCycleConfigurations>(useConfiguration);
            var claimsAccessor = MockTokenClaimsAccessor(njuadminnjuTokenClaims);

            return new BillingService(claimsAccessor, db, useTask, billingTask);
        }

        [Fact]
        public async Task TestGetCurrentAllocatedBilling()
        {
            var quota = new Domain.ValueObjects.Resources(1, 20, 30);
            nju.Quota = quota;
            db.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(nju.BillingCycleSubject));
            await db.SaveChangesAsync();

            var service = CreateService();

            var resp = await service.GetCurrentAllocatedBilling(new Protos.Billing.GetCurrentAllocatedBillingRequest
            {
                SubjectType = Protos.Common.SubjectType.Domain,
                Id = nju.Id.ToString(),
            }, TestContext);

            Assert.Equal(PricePlan.Instance.Calculate(quota), (decimal)resp.Billing.Amount);
            Assert.Equal(nju.Id.ToString(), resp.Billing.SubjectId);
            Assert.Equal(quota.ToGrpc(), resp.Billing.Resources);
            Assert.Equal(njuadmin.Id.ToString(), resp.Billing.PayerId);
        }

        [Fact]
        public async Task TestGetCurrentUsedBillings()
        {
            var resources = new Domain.ValueObjects.Resources(1, 20, 30);
            cjd67project.Resources = resources;
            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(lqproject.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(nju.UseCycleSubject));
            await db.SaveChangesAsync();

            var service = CreateService();


            async Task Validate(UseCycleSubject subject)
            {
                var resp = await service.GetCurrentUsedBilling(new GetCurrentUsedBillingRequest
                {
                    SubjectType = subject.SubjectType.ToGrpc(),
                    Id = subject.Id.ToString(),
                }, TestContext);

                Assert.Equal(PricePlan.Instance.Calculate(resources), (decimal)resp.Billing.Amount);
                Assert.Equal(subject.Id.ToString(), resp.Billing.SubjectId);
                Assert.Equal(resources.ToGrpc(), resp.Billing.Resources);
            }

            await Validate(cjd67project.UseCycleSubject);
            await Validate(lqproject.UseCycleSubject);
            await Validate(nju.UseCycleSubject);

        }
    }
}
