using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
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
    public class Project: IPayer, IReceiver, IUseCycleSubject
    {
        public Guid Id { get; set; }

        public virtual ICollection<UserProjectAssignment> Users { get; set; } = new List<UserProjectAssignment>();

        public virtual User PayUser { get; set; }
        public virtual Domain Domain { get; set; }
        
        public virtual Resources Quota { get; set; }

        public virtual Payer Payer { get; set; }

        public virtual Receiver Receiver { get; set; }
        public virtual UseCycleSubject UseCycleSubject { get; set; }


        public virtual ICollection<UseCycleRecord> UseCycleRecords => UseCycleSubject.UseCycleRecords;

        public virtual ICollection<BillingCycle> BillingCycleRecords { get; set; } = new List<BillingCycle>();

        public bool Active => Domain.Active && PayUser.Active;

        public ICollection<OrgTransaction> PayedOrgTransactions => Payer.PayedOrgTransactions;
        public ICollection<OrgTransaction> ReceivedOrgTransactions => Receiver.ReceivedOrgTransactions;

        public SubjectType SubjectType => SubjectType.Project;

        public Resources Resources => Users.Select(x => x.Resources).Aggregate(Resources.Zero, (s, a) => s + a);

        public User ReceiveUser => PayUser;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            return Receiver.Receive(from, fromUser, amount, reason);
        }

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            return Payer.Pay(receiver, amount, reason);
        }

        public void Settle(PricePlan plan, DateTime lastSettled, DateTime now)
        {
            UseCycleSubject.Settle(plan, lastSettled, now);
        }

        public Project(Guid id, User payUser, Domain domain, Resources quota)
        {
            Id = id;
            PayUser = payUser;
            Domain = domain;
            Quota = quota;

            Payer = new Payer(this);
            Receiver = new Receiver(this);
            UseCycleSubject = new UseCycleSubject(this);
        }

        public Project()
        {
        }
    }
}
