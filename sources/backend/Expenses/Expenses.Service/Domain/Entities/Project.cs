using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
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
    public class Project: IPayer, IReceiver, IUseCycleSubject, IBillingCycleSubject
    {
        public Guid Id { get; set; }

        public virtual ICollection<UserProjectAssignment> Users { get; set; } = new List<UserProjectAssignment>();

        public virtual User PayUser { get; set; }
        public virtual Domain Domain { get; set; }
        
        public virtual Resources Quota { get; set; }

        public virtual Payer Payer { get; set; }

        public virtual Receiver Receiver { get; set; }
        public virtual UseCycleSubject UseCycleSubject { get; set; }
        public virtual BillingCycleSubject BillingCycleSubject { get; set; }


        public ICollection<UseCycleRecord> UseCycleRecords => UseCycleSubject.UseCycleRecords;

        public ICollection<BillingCycleRecord> BillingCycleRecords => BillingCycleSubject.BillingCycleRecords;

        public bool Active => Domain.Active && PayUser.Active;

        public ICollection<OrgTransaction> PayedOrgTransactions => Payer.PayedOrgTransactions;
        public ICollection<OrgTransaction> ReceivedOrgTransactions => Receiver.ReceivedOrgTransactions;

        public SubjectType SubjectType => SubjectType.Project;

        public Resources Resources => Users.Select(x => x.Resources).Aggregate(Resources.Zero, (s, a) => s + a);

        public User ReceiveUser => PayUser;

        public IReceiver BillingReceiver => Domain;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason, DateTime time)
        {
            return Receiver.Receive(from, fromUser, amount, reason, time);
        }

        public OrgTransaction Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time)
        {
            return Payer.Pay(receiver, amount, reason, time);
        }

        void IUseCycleSubject.Settle(decimal price, DateTime lastSettled, DateTime now)
        {
            UseCycleSubject.Settle(price, lastSettled, now);
        }

        void IBillingCycleSubject.Settle(decimal price, DateTime lastSettled, DateTime now)
        {
            BillingCycleSubject.Settle(price, lastSettled, now);
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
            BillingCycleSubject = new BillingCycleSubject(this);
        }

        public Project()
        {
        }
    }
}
