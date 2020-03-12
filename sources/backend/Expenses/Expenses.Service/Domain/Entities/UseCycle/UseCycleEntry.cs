using AcademyCloud.Expenses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.UseCycle
{
    public class UseCycleEntry
    {
        public Guid Id { get; set; }

        public virtual UseCycleSubject Subject { get; set; }

        public DateTime LastSettled { get; set; }

        public UseCycleEntry(Guid id, UseCycleSubject subject)
        {
            Id = id;
            Subject = subject;
            LastSettled = DateTime.UtcNow;
        }

        protected UseCycleEntry()
        {
        }

        public void Settle(DateTime now)
        {
            Subject.Settle(PricePlan.Instance, LastSettled, now);
        }


    }
}
