using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
using AcademyCloud.Expenses.Domain.Services.BillingCycle;
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
        private readonly BillingCycleService service;

        public BillingCycleTask(IOptions<BillingCycleConfigurations> configuration, ScopedDbProvider provider, ILogger<BillingCycleTask> logger, BillingCycleService service)
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

                logger.LogInformation($"Start settling billing cycle at {time}..");

                await provider.WithDbContext(async dbContext =>
                {
                    foreach (var i in dbContext.BillingCycleEntries)
                    {
                        if (time >= service.NextDue(i.LastSettled))
                        {
                            if (service.TrySettle(i, i.SubjectType switch {
                                SubjectType.Domain => TransactionReason.DomainQuota,
                                SubjectType.Project => TransactionReason.ProjectQuota,
                                SubjectType.UserProjectAssignment => TransactionReason.UserProjectQuota,
                                _ => throw new InvalidOperationException($"Got {i.SubjectType} with id {i.Id} when settling billing cycle. Only domains, projects and UserProject will be settled.")
                            }))
                            {
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            logger.LogInformation($"Will not settle billing cycle {i} this time.");
                        }
                    }

                });

                logger.LogInformation("End settling billing cycle.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
