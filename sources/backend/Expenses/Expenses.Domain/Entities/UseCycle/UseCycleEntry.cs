using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
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

        public Resources Resources => Subject.Resources;
        public SubjectType SubjectType { get; set; }

        public UseCycleEntry(UseCycleSubject subject)
        {
            Id = subject.Id;
            Subject = subject;
            SubjectType = subject.SubjectType;
            LastSettled = DateTime.UtcNow;
        }

        protected UseCycleEntry()
        {
        }

        public bool Settle(decimal price, DateTime now)
        {
            LastSettled = now;
            return Subject.Settle(price, LastSettled, now);
        }


    }
}
