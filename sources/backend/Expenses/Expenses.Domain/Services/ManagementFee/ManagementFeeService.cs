using AcademyCloud.Expenses.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Domain.Services.ManagementFee
{
    public class ManagementFeeService
    {
        private readonly ManagementFeeConfigurations configurations;

        public ManagementFeeService(IOptions<ManagementFeeConfigurations> configurations)
        {
            this.configurations = configurations.Value;
        }

        public DateTime NextDue(DateTime now)
        {
            return now.AddMilliseconds(configurations.ChargeCycleMs);
        }
        private int GetPrice(ManagementFeeEntry entry)
        {
            return entry.SubjectType switch
            {
                SubjectType.Domain => configurations.DomainPrice,
                SubjectType.Project => configurations.ProjectPrice,
                SubjectType.User => configurations.UserPrice,
                _ => throw new ArgumentOutOfRangeException(nameof(entry.SubjectType))
            };
        }
        public void TrySettle(Entities.System systemEntity, ManagementFeeEntry entry)
        {
            var amount = GetPrice(entry);
            entry.Charge(systemEntity, amount, DateTime.UtcNow);

        }
    }
}
