using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.BillingCycle
{
    public class BillingCycleConfigurations
    {
        public int CheckCycleMs { get; set; }

        public int SettleCycleMs { get; set; }
    }
}
