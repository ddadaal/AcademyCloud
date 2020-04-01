using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Test.Helpers
{
    public class CombinedBillingCycleConfigurations :
        ICombinedConfigurations<Domain.Services.BillingCycle.BillingCycleConfigurations, BackgroundTasks.BillingCycle.BillingCycleConfigurations>
    {
        public int CheckCycleMs { get; set; }
        public int SettleCycleMs { get; set; }

        public Domain.Services.BillingCycle.BillingCycleConfigurations DomainConfigurations => new Domain.Services.BillingCycle.BillingCycleConfigurations
        {
            SettleCycleMs = SettleCycleMs
        };

        public BackgroundTasks.BillingCycle.BillingCycleConfigurations TaskConfigurations => new BackgroundTasks.BillingCycle.BillingCycleConfigurations
        {
            CheckCycleMs = CheckCycleMs
        };
    }
}
