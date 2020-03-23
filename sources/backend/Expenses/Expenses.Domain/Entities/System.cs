using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class System: IReceiver
    {
        public Guid Id { get; set; }

        public virtual ICollection<Domain> Domains { get; set; } = new List<Domain>();

        public virtual ICollection<User> SystemUsers { get; set; } = new List<User>();

        public virtual Receiver Receiver { get;set; }
        public virtual User ReceiveUser { get; set; }

        public SubjectType SubjectType => SubjectType.System;

        public Resources Resources => Domains.Aggregate(Resources.Zero, (a, b) => a + b.Resources);

        public ICollection<OrgTransaction> ReceivedOrgTransactions => Receiver.ReceivedOrgTransactions;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason, DateTime time)
        {
            return Receiver.Receive(from, fromUser, amount, reason, time);
        }

        public System(Guid id, User receiveUser)
        {
            Id = id;
            ReceiveUser = receiveUser;
            Receiver = new Receiver(this);
        }

        public System()
        {
        }
    }
}
