using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Billing
{
    public class CurrentAllocatedBilling: CurrentUsedBilling
    {
        public string PayerId { get; set; }

        public string PayerName { get; set; }
    }
}
