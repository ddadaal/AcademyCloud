using AcademyCloud.Expenses.Domain.Entities.Transaction;
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

        public virtual UserProjectAssignment? UserProjectAssignment { get; set; }

        public virtual ICollection<BillingCycleRecord> BillingCycleRecords { get; set; } = new List<BillingCycleRecord>();

        public SubjectType SubjectType { get; set; }

        public IBillingCycleSubject RealSubject => SubjectType switch
        {
            SubjectType.Domain => Domain!,
            SubjectType.Project => Project!,
            SubjectType.UserProjectAssignment => UserProjectAssignment!,
            _ => throw new InvalidOperationException(),
        };

        public Resources Quota => RealSubject.Quota;

        public IReceiver BillingReceiver => RealSubject.BillingReceiver;

        public bool Active => RealSubject.Active;

        public ICollection<OrgTransaction> PayedOrgTransactions => RealSubject.PayedOrgTransactions;

        public User PayUser => RealSubject.PayUser;

        public bool Settle(decimal price, DateTime lastSettled, DateTime now, TransactionReason reason)
        {
            var resources = Quota;

            if (resources == Resources.Zero) { return false; }

            var orgTransaction = RealSubject.Pay(BillingReceiver, price, reason, now);

            var cycle = new BillingCycleRecord(Guid.NewGuid(), resources, lastSettled, now, price, orgTransaction);

            BillingCycleRecords.Add(cycle);

            return true;
        }

        public OrgTransaction Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time)
        {
            return RealSubject.Pay(receiver, amount, reason, time);
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
                case UserProjectAssignment userProjectAssignment:
                    UserProjectAssignment = userProjectAssignment;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(subject));
            }


        }
    }
}
