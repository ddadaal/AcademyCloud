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

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time)
        {
            return Payer.Pay(receiver, amount, reason, time);
        }

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason, DateTime time)
        {
            return Receiver.Receive(from, fromUser, amount, reason, time);
        }

        void IUseCycleSubject.Settle(PricePlan plan, DateTime lastSettled, DateTime now)
        {
            UseCycleSubject.Settle(plan, lastSettled, now);
        }

        void IBillingCycleSubject.Settle(PricePlan pricePlan, DateTime lastSettled, DateTime now)
        {
            BillingCycleSubject.Settle(pricePlan, lastSettled, now);
        }

        #endregion

        public Domain(Guid id, User payer, Resources quota)
        {
            Id = id;
            PayUser = payer;
            Quota = quota;

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
