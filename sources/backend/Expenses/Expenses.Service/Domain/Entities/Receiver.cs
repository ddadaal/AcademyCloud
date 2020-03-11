using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class Receiver: IReceiver
    {
        public Guid Id { get; set; }

        public virtual Project? Project { get; set; }
        public virtual Domain? Domain { get; set; }

        public virtual System? System { get; set; }

        public virtual ICollection<OrgTransaction> ReceivedOrgTransactions { get; set; }

        public SubjectType SubjectType { get; set; }

        private IReceiver RealReceiver => SubjectType switch
        {
            SubjectType.User => throw new InvalidOperationException(),
            SubjectType.Project => Project!,
            SubjectType.Domain => Domain!,
            SubjectType.System => System!,
        };

        public User ReceiveUser => RealReceiver.ReceiveUser;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            var time = DateTime.UtcNow;

            var userTransaction = new UserTransaction(Guid.NewGuid(), time, amount, reason, fromUser, ReceiveUser);
            ReceiveUser.ApplyTransaction(userTransaction);

            var orgTransaction = new OrgTransaction(Guid.NewGuid(), time, amount, reason, from as Payer, this, userTransaction);
            ReceivedOrgTransactions.Add(orgTransaction);

            return orgTransaction;
        }

        public Receiver(IReceiver receiver)
        {
            Id = receiver.Id;
            SubjectType = receiver.SubjectType;
            ReceivedOrgTransactions = new List<OrgTransaction>();
            switch (receiver)
            {
                case Project project:
                    Project = project;
                    break;
                case Domain domain:
                    Domain = domain;
                    break;
                case System system:
                    System = system;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(receiver));
            }

        }
        protected Receiver()
        {
        }
    }
}
