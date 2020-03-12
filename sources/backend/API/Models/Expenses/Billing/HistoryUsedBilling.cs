using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Billing
{
    public class HistoryUsedBilling
    {
        public string Id { get; set; }

        public Resources Resources { get; set; }

        public decimal Amount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
