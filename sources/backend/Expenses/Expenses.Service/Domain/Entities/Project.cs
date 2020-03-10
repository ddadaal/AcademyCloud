using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    /// <summary>
    /// Dummy implementation to persist any type of IPayer into the database
    /// </summary>
    public class Project: IPayer, IReceiver, IBillingCycleSubject, IUseCycleSubject
    {
        public Guid Id { get; set; }

        public virtual ICollection<UserProjectAssignment> Users { get; set; } = new List<UserProjectAssignment>();

        public virtual User Payer { get; set; }
        public virtual Domain Domain { get; set; }
        
        public virtual Resources Quota { get; set; }

        public bool Active => Payer.Active;

        public virtual ICollection<UseCycle> UseCycleRecords { get; set; } = new List<UseCycle>();

        public virtual ICollection<BillingCycle> BillingCycleRecords { get; set; } = new List<BillingCycle>();

        public virtual ICollection<OrgTransaction> PayedOrgTransactions { get; set; } = new List<OrgTransaction>();
        public virtual ICollection<OrgTransaction> ReceivedOrgTransactions { get; set; } = new List<OrgTransaction>();

        public SubjectType SubjectType => SubjectType.Project;

        public Resources Resources => Users.Select(x => x.Resources).Aggregate(Resources.Zero, (s, a) => s + a);

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

        public Project(Guid id, User payer, Domain domain, Resources quota)
        {
            Id = id;
            Payer = payer;
            Domain = domain;
            Quota = quota;
        }

        public Project()
        {
        }
    }
}
