using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.UseCycle
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
