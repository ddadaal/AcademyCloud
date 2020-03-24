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
    public class Domain : IPayer, IReceiver, IUseCycleSubject, IBillingCycleSubject
    {
        #region Fields
        public Guid Id { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        public virtual ICollection<UserDomainAssignment> Users { get; set; } = new List<UserDomainAssignment>();

        public virtual System System { get; set; }

        public virtual User PayUser { get; set; }

        public virtual Resources Quota { get; set; }

        public virtual Payer Payer { get; set; }
        public virtual Receiver Receiver { get; set; }

        public virtual UseCycleSubject UseCycleSubject { get; set; }
        public virtual BillingCycleSubject BillingCycleSubject { get; set; }
        #endregion

        #region Properties and Methods

        public ICollection<UseCycleRecord> UseCycleRecords => UseCycleSubject.UseCycleRecords;
        public ICollection<BillingCycleRecord> BillingCycleRecords => BillingCycleSubject.BillingCycleRecords;

        public bool Active => PayUser.Active;


        public ICollection<OrgTransaction> PayedOrgTransactions => Payer.PayedOrgTransactions;
        public ICollection<OrgTransaction> ReceivedOrgTransactions => Receiver.ReceivedOrgTransactions;

        public SubjectType SubjectType => SubjectType.Domain;

        public Resources Resources => Projects.Select(x => x.Resources).Aggregate(Resources.Zero, (s, a) => s + a);

        public User ReceiveUser => PayUser;

        public IReceiver BillingReceiver => System;

        public OrgTransaction Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time)
        {
            return Payer.Pay(receiver, amount, reason, time);
        }

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason, DateTime time)
        {
            return Receiver.Receive(from, fromUser, amount, reason, time);
        }

        bool IUseCycleSubject.Settle(decimal price, DateTime lastSettled, DateTime now)
        {
            return UseCycleSubject.Settle(price, lastSettled, now);
        }
 
        bool IBillingCycleSubject.Settle(decimal price, Resources quota, DateTime lastSettled, DateTime now, TransactionReason reason)
        {
            return BillingCycleSubject.Settle(price, quota, lastSettled, now, reason);
        }

        #endregion

        public Domain(Guid id, User payer, Resources quota, System system)
        {
            Id = id;
            PayUser = payer;
            Quota = quota;
            System = system;

            Payer = new Payer(this);
            Receiver = new Receiver(this);
            UseCycleSubject = new UseCycleSubject(this);
            BillingCycleSubject = new BillingCycleSubject(this);
        }

        public Domain()
        {
        }
    }
}
