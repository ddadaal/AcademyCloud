using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Test.Helpers
{
    public class CombinedManagementFeeConfigurations : ICombinedConfigurations<Domain.Services.ManagementFee.ManagementFeeConfigurations, BackgroundTasks.ManagementFee.ManagementFeeConfigurations>
    {

        public int UserPrice { get; set; }

        public int ProjectPrice { get; set; }

        public int DomainPrice { get; set; }
        public int ChargeCycleMs { get; set; }
        public int CheckCycleMs { get; set; }

        public Domain.Services.ManagementFee.ManagementFeeConfigurations DomainConfigurations => new Domain.Services.ManagementFee.ManagementFeeConfigurations
        {
            ChargeCycleMs = ChargeCycleMs,
            DomainPrice = DomainPrice,
            ProjectPrice = ProjectPrice,
            UserPrice = UserPrice,
        };

        public BackgroundTasks.ManagementFee.ManagementFeeConfigurations TaskConfigurations => new BackgroundTasks.ManagementFee.ManagementFeeConfigurations
        {
            CheckCycleMs = CheckCycleMs
        };
    }
}
