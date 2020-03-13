using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
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

namespace AcademyCloud.Expenses.BackgroundTasks.UseCycle
{
    public class UseCycleTask : BackgroundService
    {
        private readonly UseCycleConfigurations configuration;
        private readonly ScopedDbProvider provider;
        private readonly ILogger<UseCycleTask> logger;

        public UseCycleTask(IOptions<UseCycleConfigurations> configuration, ScopedDbProvider provider, ILogger<UseCycleTask> logger)
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
        public bool TrySettle(UseCycleEntry entry)
        {
            if (entry.Settle(CalculatePrice(entry.Resources), DateTime.UtcNow))
            {
                logger.LogDebug($"{entry} has no resources. Skip settling.");
                return true;
            }
            else
            {
                logger.LogDebug($"Settling use cycle for {entry} completed.");
                return false;
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var time = DateTime.UtcNow;

                logger.LogDebug($"Start settling use cycle at {time}..");

                await provider.WithDbContext(async dbContext =>
                {
                    await foreach (var i in dbContext.UseCycleEntries.AsAsyncEnumerable())
                    {
                        if (time >= NextDue(i.LastSettled))
                        {
                            logger.LogDebug($"Settling use cycle for {i}");

                            if (i.Settle(CalculatePrice(i.Resources), time))
                            {
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            logger.LogDebug($"Will not settle use cycle {i} this time.");
                        }
                    }

                });

                logger.LogDebug("End settling use cycle.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
