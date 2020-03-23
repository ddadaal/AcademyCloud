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

        public virtual Resources Quota { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Amount { get; set; }

        public virtual OrgTransaction OrgTransaction { get; set; }

        public BillingCycleRecord(Guid id, Resources quota, DateTime startTime, DateTime endTime, decimal amount, OrgTransaction orgTransaction)
        {
            Id = id;
            Quota = quota;
            StartTime = startTime;
            EndTime = endTime;
            Amount = amount;
            OrgTransaction = orgTransaction;
        }

        protected BillingCycleRecord() { }

    }
}
