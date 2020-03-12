using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.BillingCycle
{
    public class BillingCycleRecord
    {
        public Guid Id { get; set; }

        public virtual Resources Resources { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Amount { get; set; }

        public BillingCycleRecord(Guid id, Resources resources, DateTime startTime, DateTime endTime, decimal amount)
        {
            Id = id;
            Resources = resources;
            StartTime = startTime;
            EndTime = endTime;
            Amount = amount;
        }

        protected BillingCycleRecord() { }

    }
}
