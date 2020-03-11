using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
{
    public class UseCycleTests : CommonTest
    {
        private UseCycleConfiguration configuration = new UseCycleConfiguration
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };

        private UseCycleTask CreateTask()
        {
            return ConfigureTask<UseCycleTask, UseCycleConfiguration>(configuration);
        }

        private async Task Wait(int waitTimes = 1)
        {
            await WaitForTaskForExecuteCycles(CreateTask(), configuration.CheckCycleMs, waitTimes);
        }

        [Fact]
        public async Task TestSimple()
        {
            // add cjd to use 1 CPU, 2 GB RAM and 40 Storage on 67 project
            cjd67project.Resources = new Resources(1, 2, 40);
            db.UseCycleEntries.Add(new UseCycleEntry(Guid.NewGuid(), cjd67project.UseCycleSubject));
            await db.SaveChangesAsync();

            // Wait 2 cycles to settle once.
            await Wait(2);

            // Check cjd 67 project 
            Assert.Single(cjd67project.UseCycleRecords);
            var record = cjd67project.UseCycleRecords.First();
            Assert.Equal(new Resources(1, 2, 40), record.Resources);
            Assert.Equal(PricePlan.Instance.Calculate(new Resources(1, 2, 40)), record.Amount);

        }
    }
}
