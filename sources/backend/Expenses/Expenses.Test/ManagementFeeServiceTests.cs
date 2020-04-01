using AcademyCloud.Expenses.BackgroundTasks.ManagementFee;
using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Services.ManagementFee;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
{
    public class ManagementFeeServiceTests : CommonTest
    {

        private CombinedManagementFeeConfigurations configuration = new CombinedManagementFeeConfigurations
        {
            CheckCycleMs = 500,
            ChargeCycleMs = 1000,
            UserPrice = 5,
            DomainPrice = 10,
            ProjectPrice = 15,
        };

        private ManagementFeeTask CreateTask()
        {
            return ConfigureManagementFeeTask(configuration).Item1;
        }

        private async Task Wait(int waitTimes = 1)
        {
            await WaitForTaskForExecuteCycles(CreateTask(), configuration.CheckCycleMs, waitTimes);
        }

        [Fact]
        public async Task TestUserPayManagmentFee()
        {
            var previousBalance = cjd.Balance;

            // Add cjd into the ManagementFeeEntry
            db.ManagementFeeEntries.Add(new ManagementFeeEntry(cjd.Payer));
            db.SaveChanges();

            // Wait 2 check cycles (1 charge cycle)
            await Wait(2);

            Assert.Equal(previousBalance - configuration.UserPrice, cjd.Balance);
        }

        [Fact]
        public async Task TestDomainPayManagementFee()
        {
            // Add nju into the ManagementFeeEntry
            var previousBalance = njuadmin.Balance;

            db.ManagementFeeEntries.Add(new ManagementFeeEntry(nju.Payer));
            db.SaveChanges();

            // Wait 2 check cycles (1 charge cycle)
            await Wait(2);

            Assert.Equal(previousBalance - configuration.DomainPrice, njuadmin.Balance);
        }

        [Fact]
        public async Task TestProjectPayManagementFee()
        {
            // Add nju into the ManagementFeeEntry
            var previousBalance = fc.Balance;

            db.ManagementFeeEntries.Add(new ManagementFeeEntry(fcproject.Payer));
            db.SaveChanges();

            // Wait 2 check cycles (1 charge cycle)
            await Wait(2);

            Assert.Equal(previousBalance - configuration.ProjectPrice, fc.Balance);
        }

        [Fact]
        public async Task TestExecuteThreeTimes()
        {

            // Add nju into the ManagementFeeEntry
            var previousBalance = njuadmin.Balance;

            db.ManagementFeeEntries.Add(new ManagementFeeEntry(nju.Payer));
            db.SaveChanges();

            // wait 3 check cycles
            await Wait(3);

            // Will only pass 1 charge cycle
            Assert.Equal(previousBalance - configuration.DomainPrice, njuadmin.Balance);
        }
    }
}
