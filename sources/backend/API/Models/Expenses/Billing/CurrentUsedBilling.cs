using AcademyCloud.API.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Billing
{
    public class CurrentUsedBilling
    {
        public string SubjectId { get; set; }

        public string SubjectName { get; set; }

        public Resources Resources { get; set; }

        public decimal Amount { get; set; }

        public DateTime NextDue { get; set; }
    }
}
