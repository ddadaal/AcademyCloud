using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Billing;
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

        public (BillingService, BillingCycleTask, UseCycleTask) CreateService()
        {
            var billingTask = ConfigureTask<BillingCycleTask, BillingCycleConfigurations>(billingConfiguration);
            var useTask = ConfigureTask<UseCycleTask, UseCycleConfigurations>(useConfiguration);
            var claimsAccessor = MockTokenClaimsAccessor(njuadminnjuTokenClaims);

            return (new BillingService(claimsAccessor, db, useTask, billingTask), billingTask, useTask);
        }

        [Fact]
        public async Task TestGetCurrentAllocatedBilling()
        {
            var quota = new Domain.ValueObjects.Resources(1, 20, 30);
            nju.Quota = quota;
            db.BillingCycleEntries.Add(new BillingCycleEntry(nju.BillingCycleSubject));
            await db.SaveChangesAsync();

            var (service, _, _) = CreateService();

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

            var (service, _, _) = CreateService();


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

        [Fact]
        public async Task TestGetCurrentAllocatedBillings()
        {
            var quota = new Domain.ValueObjects.Resources(1, 20, 30);
            nju.Quota = quota;
            cjd67project.Quota = quota;
            db.BillingCycleEntries.Add(new BillingCycleEntry(nju.BillingCycleSubject));
            db.BillingCycleEntries.Add(new BillingCycleEntry(cjd67project.BillingCycleSubject));
            await db.SaveChangesAsync();



        }

        [Fact]
        public async Task TestGetHistoryUsedBillings()
        {
            var usedResources = new Domain.ValueObjects.Resources(5, 10, 15);
            cjd67project.Resources = usedResources;

            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(lqproject.UseCycleSubject));

            var (service, _, useTask) = CreateService();

            await WaitForTaskForExecuteCycles(useTask, useConfiguration.CheckCycleMs, 2);

            var resp = await service.GetHistoryUsedBillings(new GetHistoryUsedBillingsRequest
            {
                Id = cjd67project.Id.ToString(),
                SubjectType = Protos.Common.SubjectType.UserProjectAssignment
            }, TestContext);

            Assert.Single(resp.Billings);

            var billing = resp.Billings.First();
            Assert.Equal(usedResources.ToGrpc(), billing.Resources);
            Assert.Equal(PricePlan.Instance.Calculate(usedResources), (decimal)billing.Amount);
        }

        [Fact]
        public async Task TestGetHistoryAllocatedBillings()
        {
            var quota = new Domain.ValueObjects.Resources(5, 10, 15);
            lqproject.Quota = quota;

            db.BillingCycleEntries.Add(new BillingCycleEntry(lqproject.BillingCycleSubject));

            var (service, billingTask, _) = CreateService();

            // wait 2 cycles
            await WaitForTaskForExecuteCycles(billingTask, billingConfiguration.CheckCycleMs, 2 * 2);

            var resp = await service.GetHistoryAllocatedBillings(new GetHistoryAllocatedBillingsRequest
            {
                Id = lqproject.Id.ToString(),
                SubjectType = Protos.Common.SubjectType.Project,
            }, TestContext);

            Assert.Equal(2, resp.Billings.Count);

            var billing = resp.Billings.First();
            Assert.Equal(quota.ToGrpc(), billing.Resources);
            Assert.Equal(PricePlan.Instance.Calculate(quota), (decimal)billing.Amount);
        }
    }
}
