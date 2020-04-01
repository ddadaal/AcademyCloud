using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Domain.Services.BillingCycle
{
    public class BillingCycleService
    {
        private readonly BillingCycleConfigurations configurations;

        public BillingCycleService(IOptions<BillingCycleConfigurations> configurations)
        {
            this.configurations = configurations.Value;
        }

        public decimal CalculatePrice(Resources resources)
        { 
            return PricePlan.Instance.Calculate(resources);
        }

        public DateTime NextDue(DateTime now)
        {
            return now.AddMilliseconds(configurations.SettleCycleMs);
        }

        public bool TrySettle(BillingCycleEntry entry, TransactionReason reason)
        {
            // if the entry is a social project, use its resources instead of quota
            var quota = entry.Subject.Project != null && entry.Subject.Project.Domain.Id == Shared.Constants.SocialDomainId
                ? entry.Subject.Project.Resources
                : entry.Quota;

            // if the entry is a UserProjectAssignment, the price should always to zero
            var price = entry.SubjectType == SubjectType.UserProjectAssignment
                ? 0
                : CalculatePrice(quota);

            return entry.Settle(price, quota, DateTime.UtcNow, reason);
        }
    }
}
