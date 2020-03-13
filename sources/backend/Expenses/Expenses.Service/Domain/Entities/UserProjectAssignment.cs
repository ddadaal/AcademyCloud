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
    public class UserProjectAssignment: IPayer, IUseCycleSubject, IBillingCycleSubject
    {

        public Guid Id { get; set; }
        public virtual Project Project { get; set; }

        public virtual User User { get; set; }

        public virtual Resources Quota { get; set; }
        public virtual Resources Resources { get; set; } = Resources.Zero;

        public virtual Payer Payer { get; set; }

        public virtual UseCycleSubject UseCycleSubject { get; set; }

        public virtual BillingCycleSubject BillingCycleSubject { get; set; }

        public ICollection<UseCycleRecord> UseCycleRecords => UseCycleSubject.UseCycleRecords;
        public ICollection<BillingCycleRecord> BillingCycleRecords => BillingCycleSubject.BillingCycleRecords;

        public SubjectType SubjectType => SubjectType.UserProjectAssignment;

        public IReceiver BillingReceiver => Project;

        public bool Active => User.Active && Project.Active;

        public ICollection<OrgTransaction> PayedOrgTransactions => Payer.PayedOrgTransactions;

        public User PayUser => User;

        public UserProjectAssignment(Guid id, User user, Project project,  Resources quota)
        {
            Id = id;
            User = user;
            Project = project;
            Quota = quota;

            Payer = new Payer(this);
            UseCycleSubject = new UseCycleSubject(this);
            BillingCycleSubject = new BillingCycleSubject(this);
        }

        public UserProjectAssignment()
        {
        }

        bool IUseCycleSubject.Settle(decimal price, DateTime lastSettled, DateTime now)
        {
           return UseCycleSubject.Settle(price, lastSettled, now);
        }

        bool IBillingCycleSubject.Settle(decimal price, DateTime lastSettled, DateTime now, TransactionReason reason)
        {
            return BillingCycleSubject.Settle(price, lastSettled, now, reason);
        }

        public OrgTransaction Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time)
        {
            return Payer.Pay(receiver, amount, reason, time);
        }
    }
}
