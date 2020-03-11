using AcademyCloud.Expenses.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Extensions
{
    public class ScopedDbProvider
    {
        private IServiceProvider provider;

        public ScopedDbProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async Task WithDbContext(Func<ExpensesDbContext, Task> action)
        {
            using var scope = provider.CreateScope();

            var db = provider.GetService<ExpensesDbContext>();

            await action(db);

        }
    }
}
