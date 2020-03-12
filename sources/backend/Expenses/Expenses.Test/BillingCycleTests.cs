using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
{
    public class BillingCycleTests : CommonTest
    {
        private BillingCycleConfigurations configuration = new BillingCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };

        private BillingCycleTask CreateTask()
        {
            return ConfigureTask<BillingCycleTask, BillingCycleConfigurations>(configuration);
        }

        private async Task Wait(int waitTimes = 1)
        {
            await WaitForTaskForExecuteCycles(CreateTask(), configuration.CheckCycleMs, waitTimes);
        }

        [Fact]
        public async Task TestSimple()
        {
            var quota = new Resources(1, 2, 40);
            var previousLqBalance = lq.Balance;
            var previousNjuadminBalance = njuadmin.Balance;
            // add 1 CPU, 2 GB RAM and 40 Storage quota on 67 project
            lqproject.Quota = quota;
            db.BillingCycleEntries.Add(new BillingCycleEntry(Guid.NewGuid(), lqproject.BillingCycleSubject));
            await db.SaveChangesAsync();

            // Wait 2 cycles to settle once.
            await Wait(2);

            // Check 67 project 
            Assert.Single(lqproject.BillingCycleRecords);
            var record = lqproject.BillingCycleRecords.First();
            var expectedPrice = PricePlan.Instance.Calculate(quota);
            Assert.Equal(quota, record.Quota);
            Assert.Equal(expectedPrice, record.Amount);
            Assert.Equal(previousLqBalance - expectedPrice, lq.Balance);
            Assert.Equal(previousNjuadminBalance + expectedPrice, njuadmin.Balance);
        }


        // Need great overhalt to test design
        // Don't want to do it anymore
        //[Fact]
        //public async Task TestBillingrResourcesChange()
        //{
        //    var resources = new Resources(1, 2, 40);
        //    // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
        //    cjd67project.Resources = resources.Clone();
        //    // Register NJU domain, lqproject Project and CJD on lq Billingr
        //    db.BillingCycleEntries.Add(new BillingCycleEntry(Guid.NewGuid(), cjd67project.BillingCycleSubject));
        //    db.BillingCycleEntries.Add(new BillingCycleEntry(Guid.NewGuid(), lqproject.BillingCycleSubject));
        //    db.BillingCycleEntries.Add(new BillingCycleEntry(Guid.NewGuid(), nju.BillingCycleSubject));
        //    await db.SaveChangesAsync();

        //    // Create task
        //    var task = CreateTask();
        //    await task.StartAsync(CancellationToken.None);

        //    // 1. Settle cjd once
        //    await WaitForTaskForExecuteCycles(task, configuration.CheckCycleMs, 2);

        //    // 2. Change 67 resources use
        //    db.BillingrProjectAssignments.Find(lq67project.Id).Resources = 2 * resources.Clone();
        //    await db.SaveChangesAsync();

        //    // 2. settle once more
        //    await WaitForTaskForExecuteCycles(task, configuration.CheckCycleMs, 2);

        //    // Stop
        //    await task.StopAsync(CancellationToken.None);

        //    // Check 
        //    Assert.Equal(4 * resources, db.Projects.Find(lqproject.Id).BillingCycleRecords.OrderByDescending(x => x.EndTime).First().Resources);
        //}
    }
}
