using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.BillingCycle
{
    public class BillingCycleSubject : IBillingCycleSubject
    {
        public Guid Id { get; set; }

        public virtual Domain? Domain { get; set; }

        public virtual Project? Project { get; set; }

        public virtual ICollection<BillingCycleRecord> BillingCycleRecords { get; set; } = new List<BillingCycleRecord>();

        public SubjectType SubjectType { get; set; }

        public IBillingCycleSubject RealSubject => SubjectType switch
        {
            SubjectType.Domain => Domain!,
            SubjectType.Project => Project!,
            _ => throw new InvalidOperationException(),
        };

        public Resources Quota => RealSubject.Quota;


        public void Settle(PricePlan plan, DateTime lastSettled, DateTime now)
        {
            var resources = Quota;

            var price = plan.Calculate(resources);

            var cycle = new BillingCycleRecord(Guid.NewGuid(), resources, lastSettled, now, price);

            BillingCycleRecords.Add(cycle);

        }

        protected BillingCycleSubject() { }

        public BillingCycleSubject(IBillingCycleSubject subject)
        {
            Id = subject.Id;
            SubjectType = subject.SubjectType;

            switch (subject)
            {
                case Domain domain:
                    Domain = domain;
                    break;
                case Project project:
                    Project = project;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(subject));
            }


        }
    }
}
