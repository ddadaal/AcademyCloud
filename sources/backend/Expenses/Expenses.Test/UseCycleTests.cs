using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
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
    public class UseCycleTests : CommonTest
    {
        private UseCycleConfigurations configuration = new UseCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };

        private UseCycleTask CreateTask()
        {
            return ConfigureTask<UseCycleTask, UseCycleConfigurations>(configuration);
        }

        private async Task Wait(int waitTimes = 1)
        {
            await WaitForTaskForExecuteCycles(CreateTask(), configuration.CheckCycleMs, waitTimes);
        }

        [Fact]
        public async Task TestSimple()
        {
            var resources = new Resources(1, 2, 40);
            // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
            cjd67project.Resources = resources;
            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            await db.SaveChangesAsync();

            // Wait 2 cycles to settle once.
            await Wait(2);

            // Check cjd 67 project 
            Assert.Single(cjd67project.UseCycleRecords);
            var record = cjd67project.UseCycleRecords.First();
            Assert.Equal(resources, record.Resources);
            Assert.Equal(PricePlan.Instance.Calculate(resources), record.Amount);
        }

        [Fact]
        public async Task TestAllOfDomainProjectAndUserUseCycle()
        {
            var resources = new Resources(1, 2, 40);
            // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
            cjd67project.Resources = resources;
            // Register NJU domain, lqproject Project and CJD on lq User
            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(lqproject.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(nju.UseCycleSubject));
            await db.SaveChangesAsync();

            // Wait 2 cycles to settle once.
            await Wait(2);

            void AssertRecords(IEnumerable<UseCycleRecord> records)
            {
                Assert.Single(records);
                var record = records.First();
                Assert.Equal(resources, record.Resources);
                Assert.Equal(PricePlan.Instance.Calculate(resources), record.Amount);
            }

            // Check cjd on 67 project 
            AssertRecords(cjd67project.UseCycleRecords);

            // Check lq project
            AssertRecords(lqproject.UseCycleRecords);

            // Check nju  domain
            AssertRecords(nju.UseCycleRecords);
        }

        [Fact]
        public async Task TestMultipleUsersOnOneProject()
        {
            var resources = new Resources(1, 2, 40);
            // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
            cjd67project.Resources = resources.Clone();
            lq67project.Resources = resources.Clone();
            // Register NJU domain, lqproject Project and CJD on lq User
            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(lqproject.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(nju.UseCycleSubject));
            await db.SaveChangesAsync();

            await Wait(2);

            // Check lq project should be the sum of the resources used by two users
            Assert.Equal(2 * resources, lqproject.UseCycleRecords.First().Resources);

            // Check nju domain
            Assert.Equal(2 * resources, nju.UseCycleRecords.First().Resources);

        }

        [Fact]
        public async Task TestZeroQuota()
        {
            var resources = Resources.Zero;
            // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
            cjd67project.Resources = resources.Clone();
            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            await db.SaveChangesAsync();

            // Wait 3 cycles to settle once.
            await Wait(3);

            // zero resources means no settled
            Assert.Empty(cjd67project.UseCycleRecords);
        }

        // Need great overhalt to test design
        // Don't want to do it anymore
        //[Fact]
        //public async Task TestUserResourcesChange()
        //{
        //    var resources = new Resources(1, 2, 40);
        //    // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
        //    cjd67project.Resources = resources.Clone();
        //    // Register NJU domain, lqproject Project and CJD on lq User
        //    db.UseCycleEntries.Add(new UseCycleEntry(Guid.NewGuid(), cjd67project.UseCycleSubject));
        //    db.UseCycleEntries.Add(new UseCycleEntry(Guid.NewGuid(), lqproject.UseCycleSubject));
        //    db.UseCycleEntries.Add(new UseCycleEntry(Guid.NewGuid(), nju.UseCycleSubject));
        //    await db.SaveChangesAsync();

        //    // Create task
        //    var task = CreateTask();
        //    await task.StartAsync(CancellationToken.None);

        //    // 1. Settle cjd once
        //    await WaitForTaskForExecuteCycles(task, configuration.CheckCycleMs, 2);

        //    // 2. Change 67 resources use
        //    db.UserProjectAssignments.Find(lq67project.Id).Resources = 2 * resources.Clone();
        //    await db.SaveChangesAsync();

        //    // 2. settle once more
        //    await WaitForTaskForExecuteCycles(task, configuration.CheckCycleMs, 2);

        //    // Stop
        //    await task.StopAsync(CancellationToken.None);

        //    // Check 
        //    Assert.Equal(4 * resources, db.Projects.Find(lqproject.Id).UseCycleRecords.OrderByDescending(x => x.EndTime).First().Resources);
        //}
    }
}
