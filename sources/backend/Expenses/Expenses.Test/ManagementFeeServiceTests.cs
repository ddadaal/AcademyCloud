using AcademyCloud.Expenses.BackgroundTasks.ManagementFee;
using AcademyCloud.Expenses.Data;
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

        private ManagementFeeConfiguration configuration = new ManagementFeeConfiguration
        {
            CheckCycleMs = 500,
            ChargeCycleMs = 1000,
        };

        private ManagementFeeTask CreateTask()
        {
            var mockIOptions = new Mock<IOptions<ManagementFeeConfiguration>>();
            mockIOptions.Setup(x => x.Value).Returns(configuration);

            var services = new ServiceCollection();

            services.AddSingleton(db);
            services.AddLogging(o => o.AddConsole());
            services.AddSingleton(mockIOptions.Object);
            services.AddSingleton<ManagementFeeTask>();

            var provider = services.BuildServiceProvider();

            return provider.GetService<ManagementFeeTask>();
        }

        private async Task Wait(int waitTimes = 1)
        {
            var task = CreateTask();

            var token = new CancellationToken();

            await task.StartAsync(token);

            for (int i = 0; i < waitTimes; i++)
            {
                await Task.Delay(configuration.CheckCycleMs + 100, token);
            }

            await task.StopAsync(token);
        }

        [Fact]
        public async Task TestUserPayManagmentFee()
        {
            var previousBalance = cjd.Balance;
            var price = 5;

            // Add cjd into the ManagementFeeEntry
            db.ManagementFeeEntries.Add(new ManagementFeeEntry(Guid.NewGuid(), cjd.Payer, price));
            db.SaveChanges();

            // Wait 2 check cycles (1 charge cycle)
            await Wait(2);

            Assert.Equal(previousBalance - price, cjd.Balance);
        }

        [Fact]
        public async Task TestDomainPayManagementFee()
        {
            // Add nju into the ManagementFeeEntry
            var previousBalance = njuadmin.Balance;
            var price = 5;

            db.ManagementFeeEntries.Add(new ManagementFeeEntry(Guid.NewGuid(), nju.Payer, price));
            db.SaveChanges();

            // Wait 2 check cycles (1 charge cycle)
            await Wait(2);

            Assert.Equal(previousBalance - price, njuadmin.Balance);
        }

        [Fact]
        public async Task TestProjectPayManagementFee()
        {
            // Add nju into the ManagementFeeEntry
            var previousBalance = fc.Balance;
            var price = 5;

            db.ManagementFeeEntries.Add(new ManagementFeeEntry(Guid.NewGuid(), fcproject.Payer, price));
            db.SaveChanges();

            // Wait 2 check cycles (1 charge cycle)
            await Wait(2);

            Assert.Equal(previousBalance - price, fc.Balance);
        }

        [Fact]
        public async Task TestExecuteThreeTimes()
        {

            // Add nju into the ManagementFeeEntry
            var previousBalance = njuadmin.Balance;
            var price = 5;

            db.ManagementFeeEntries.Add(new ManagementFeeEntry(Guid.NewGuid(), nju.Payer, price));
            db.SaveChanges();

            // wait 3 check cycles
            await Wait(3);

            // Will only pass 1 charge cycle
            Assert.Equal(previousBalance - price, njuadmin.Balance);
        }
    }
}
