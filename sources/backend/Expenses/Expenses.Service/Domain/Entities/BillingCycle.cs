using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class BillingCycle
    {
        public Guid Id { get; set; }

        public virtual Resources Resources { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Amount { get; set; }

        public virtual User Payer { get; set; }
    }
}
