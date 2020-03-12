using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.BillingCycle
{
    public class BillingCycleTask : BackgroundService
    {
        private readonly BillingCycleConfigurations configuration;
        private readonly ScopedDbProvider provider;
        private readonly ILogger<BillingCycleTask> logger;

        public BillingCycleTask(IOptions<BillingCycleConfigurations> configuration, ScopedDbProvider provider, ILogger<BillingCycleTask> logger)
        {
            this.configuration = configuration.Value;
            this.provider = provider;
            this.logger = logger;
        }

        public DateTime NextDue(DateTime now)
        {
            return now.AddMilliseconds(configuration.SettleCycleMs);
        }
        public decimal CalculatePrice(Resources resources)
        {
            return PricePlan.Instance.Calculate(resources);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var time = DateTime.UtcNow;

                logger.LogDebug($"Start settling billing cycle at {time}..");

                await provider.WithDbContext(async dbContext =>
                {
                    await foreach (var i in dbContext.BillingCycleEntries.AsAsyncEnumerable())
                    {
                        if (time >= NextDue(i.LastSettled))
                        { 
                            i.Settle(CalculatePrice(i.Quota), time);

                            logger.LogDebug($"Settling billing cycle for {i} completed.");
                        }
                        else
                        {
                            logger.LogDebug($"Will not settle billing cycle {i} this time.");
                        }
                    }

                    await dbContext.SaveChangesAsync();
                });

                logger.LogDebug("End settling billing cycle.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
