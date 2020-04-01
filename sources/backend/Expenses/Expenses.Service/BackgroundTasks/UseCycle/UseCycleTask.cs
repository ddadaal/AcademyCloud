using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using AcademyCloud.Expenses.Domain.Services.UseCycle;
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
        private readonly UseCycleService service;

        public UseCycleTask(IOptions<UseCycleConfigurations> configuration, ScopedDbProvider provider, ILogger<UseCycleTask> logger, UseCycleService service)
        {
            this.configuration = configuration.Value;
            this.provider = provider;
            this.logger = logger;
            this.service = service;
        }




        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var time = DateTime.UtcNow;

                logger.LogInformation($"Start settling use cycle at {time}..");

                await provider.WithDbContext(async dbContext =>
                {
                    foreach (var i in dbContext.UseCycleEntries)
                    {
                        if (time >= service.NextDue(i.LastSettled))
                        {
                            logger.LogInformation($"Settling use cycle for {i}");

                            if (service.TrySettle(i))
                            {
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            logger.LogInformation($"Will not settle use cycle {i} this time.");
                        }
                    }

                });

                logger.LogInformation("End settling use cycle.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
