using AcademyCloud.Expenses.Domain.ValueObjects;
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
        
        public SubjectType SubjectType { get; set; }

        public Resources Quota => Subject.Quota;

        public BillingCycleEntry(BillingCycleSubject subject)
        {
            Id = subject.Id;
            Subject = subject;
            SubjectType = subject.SubjectType;
            LastSettled = DateTime.UtcNow;
        }

        protected BillingCycleEntry()
        {
        }

        public bool Settle(decimal price, Resources quota, DateTime now, TransactionReason reason)
        {
            LastSettled = now;
            return Subject.Settle(price, quota, LastSettled, now, reason);
        }


    }
}
