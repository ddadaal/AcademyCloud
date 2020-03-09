using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class Domain: IPayer, IReceiver, IBillingCycleSubject, IUseCycleSubject
    {
        public Guid Id { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual User Payer { get; set; }

        public virtual Resources Quota { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<UseCycle> UseCycleRecords { get; set; }

        public virtual ICollection<BillingCycle> BillingCycleRecords { get; set; }

        public SubjectType SubjectType => SubjectType.Domain;

        public Resources Resources => Projects.Select(x => x.Resources).Aggregate(Resources.Zero, (s, a) => s + a);

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            throw new NotImplementedException();
        }

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            throw new NotImplementedException();
        }

        void IBillingCycleSubject.Settle(Resources quota, decimal price)
        {
            throw new NotImplementedException();
        }

        void IUseCycleSubject.Settle(Resources resources, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
