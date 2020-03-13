using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.ManagementFee
{
    public class ManagementFeeConfigurations
    {
        public int CheckCycleMs { get; set; }
        public int ChargeCycleMs { get; set; }
        
        public int UserPrice { get; set; }

        public int ProjectPrice { get; set; }

        public int DomainPrice { get; set; }
    }
}
