using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.BillingCycle
{
    public class BillingCycleEntry
    {
        public Guid Id { get; set; }

        public virtual BillingCycleSubject Subject { get; set; }

        public DateTime LastSettled { get; set; }

        public BillingCycleEntry(Guid id, BillingCycleSubject subject)
        {
            Id = id;
            Subject = subject;
            LastSettled = DateTime.UtcNow;
        }

        protected BillingCycleEntry()
        {
        }

        public void Settle(DateTime now)
        {
            Subject.Settle(PricePlan.Instance, LastSettled, now);
        }


    }
}
