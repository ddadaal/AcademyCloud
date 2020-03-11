using AcademyCloud.Expenses.Domain.Entities;
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
        private readonly UseCycleConfiguration configuration;
        private readonly ScopedDbProvider provider;
        private readonly ILogger<UseCycleTask> logger;

        public UseCycleTask(IOptions<UseCycleConfiguration> configuration, ScopedDbProvider provider, ILogger<UseCycleTask> logger)
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

                logger.LogDebug($"Start settling use cycle at {time}..");

                await provider.WithDbContext(async dbContext =>
                {
                    await foreach (var i in dbContext.UseCycleEntries.AsAsyncEnumerable())
                    {
                        if ((time - i.LastSettled).TotalMilliseconds > configuration.SettleCycleMs)
                        {
                            logger.LogDebug($"Settling use cycle for {i}");

                            i.Settle(time);

                            logger.LogDebug($"Settling use cycle for {i} completed.");
                        }
                        else
                        {
                            logger.LogDebug($"Will not settle use cycle {i} this time.");
                        }
                    }

                    await dbContext.SaveChangesAsync();
                });

                logger.LogDebug("End settling use cycle.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
