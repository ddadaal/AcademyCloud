using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Extensions
{
    public class DatabaseInitializerHostedService : IHostedService
    {
        private readonly ScopedDbProvider provider;

        public DatabaseInitializerHostedService(ScopedDbProvider provider)
        {
            this.provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await provider.WithDbContext(async db =>
            {
                await db.Database.EnsureCreatedAsync();

            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
