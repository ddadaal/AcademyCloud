using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Services.ManagementFee;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Extensions;
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
        private readonly ManagementFeeConfigurations configuration;
        private readonly ScopedDbProvider provider;
        private readonly ILogger<ManagementFeeTask> logger;
        private readonly ManagementFeeService service;

        public ManagementFeeTask(IOptions<ManagementFeeConfigurations> configuration, ScopedDbProvider provider, ILogger<ManagementFeeTask> logger, ManagementFeeService service)
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
                logger.LogInformation($"Start charging management fees at {time}..");

                await provider.WithDbContext(async dbContext =>
                {
                    var system = await dbContext.Systems.FirstAsync();

                    foreach (var i in dbContext.ManagementFeeEntries)
                    {
                        if (time >= service.NextDue(i.LastSettled))
                        {
                            logger.LogInformation($"{i} is being charged with management fee.");
                            service.TrySettle(system, i);

                            logger.LogInformation($"Charging {i} for management fee is completed.");
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            logger.LogInformation($"{i} will not be charged with management fee this time.");
                        }
                    }

                });

                logger.LogInformation("End charging management fee.");

                await Task.Delay(configuration.CheckCycleMs, stoppingToken);
            }
        }
    }
}
