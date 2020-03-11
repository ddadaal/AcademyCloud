using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.ManagementFee
{
    public class ManagementFeeTask : BackgroundService
    {
        private readonly ManagementFeeConfiguration configuration;
        private readonly IServiceProvider provider;
        private readonly ILogger<ManagementFeeTask> logger;

        public ManagementFeeTask(IOptions<ManagementFeeConfiguration> configuration, IServiceProvider provider, ILogger<ManagementFeeTask> logger)
        {
            this.configuration = configuration.Value;
            this.provider = provider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var time = DateTime.UtcNow;

                logger.LogDebug($"Start charging management fees at {time}..");

                using var scope = provider.CreateScope();

                var dbContext = scope.ServiceProvider.GetService<ExpensesDbContext>();

                var system = await dbContext.Systems.FirstAsync();


                await foreach (var i in dbContext.ManagementFeeEntries.AsAsyncEnumerable())
                {
                    if ((time - i.LastSettled).TotalMilliseconds > configuration.ChargeCycleMs)
                    {
                        logger.LogDebug($"{i} is being charged with management fee {i.Amount}.");

                        i.Charge(system, time);

                        logger.LogDebug($"Charging {i} for management fee is completed.");
                    }
                    else
                    {
                        logger.LogDebug($"{i} will not be charged with management fee this time.");
                    }
                }

                await dbContext.SaveChangesAsync();

                logger.LogDebug("End charging management fee.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
