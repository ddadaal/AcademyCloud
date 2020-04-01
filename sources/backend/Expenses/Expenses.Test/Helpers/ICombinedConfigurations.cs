using System;
using System.Collections.Generic;
using System.Text;

namespace AcademyCloud.Expenses.Test.Helpers
{
    public interface ICombinedConfigurations<TDomainConfigurations, TTaskConfigurations>
    {
        TDomainConfigurations DomainConfigurations { get; }
        
        TTaskConfigurations TaskConfigurations { get; }
    }
}
