using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Domain.Services.ManagementFee
{
    public class ManagementFeeConfigurations
    {
        public int UserPrice { get; set; }

        public int ProjectPrice { get; set; }

        public int DomainPrice { get; set; }
        public int ChargeCycleMs { get; set; }
    }
}
