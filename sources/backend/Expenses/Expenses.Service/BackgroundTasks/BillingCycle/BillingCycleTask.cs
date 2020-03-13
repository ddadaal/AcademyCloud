using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
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

        public decimal CalculatePrice(BillingCycleEntry entry)
        {
            if (entry.SubjectType == SubjectType.UserProjectAssignment)
            {
                return 0;
            }
            else
            {
                return CalculatePrice(entry.Quota);
            }
        }

        public decimal CalculatePrice(Resources resources)
        {
            return PricePlan.Instance.Calculate(resources);
        }
        public bool TrySettle(BillingCycleEntry entry, TransactionReason reason)
        {
            if (entry.Settle(CalculatePrice(entry), DateTime.UtcNow, reason))
            {
                logger.LogDebug($"Settling billing cycle for {entry} completed.");
                return true;
            }
            else
            {
                logger.LogDebug($"{entry} has no quota. Skip settling.");
                return false;
            }
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
                            if (TrySettle(i, TransactionReason.DomainResources))
                            {
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            logger.LogDebug($"Will not settle billing cycle {i} this time.");
                        }
                    }

                });

                logger.LogDebug("End settling billing cycle.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
