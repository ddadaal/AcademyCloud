using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Test.Helpers
{
    public class CombinedUseCycleConfigurations :
        ICombinedConfigurations<Domain.Services.UseCycle.UseCycleConfigurations, BackgroundTasks.UseCycle.UseCycleConfigurations>
    {
        public int CheckCycleMs { get; set; }
        public int SettleCycleMs { get; set; }

        public Domain.Services.UseCycle.UseCycleConfigurations DomainConfigurations => new Domain.Services.UseCycle.UseCycleConfigurations
        {
            SettleCycleMs = SettleCycleMs
        };

        public BackgroundTasks.UseCycle.UseCycleConfigurations TaskConfigurations => new BackgroundTasks.UseCycle.UseCycleConfigurations
        {
            CheckCycleMs = CheckCycleMs
        };
    }
}
