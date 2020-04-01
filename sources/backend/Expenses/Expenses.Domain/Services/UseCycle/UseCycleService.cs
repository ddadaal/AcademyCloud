using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Domain.Services.UseCycle
{
    public class UseCycleService
    {
        private readonly UseCycleConfigurations configurations;

        public UseCycleService(IOptions<UseCycleConfigurations> configurations)
        {
            this.configurations = configurations.Value;
        }
        public DateTime NextDue(DateTime now)
        {
            return now.AddMilliseconds(configurations.SettleCycleMs);
        }

        public decimal CalculatePrice(Resources resources)
        {
            return PricePlan.Instance.Calculate(resources);
        }
        public bool TrySettle(UseCycleEntry entry)
        {
            return entry.Settle(CalculatePrice(entry.Resources), DateTime.UtcNow);
        }
    }
}
