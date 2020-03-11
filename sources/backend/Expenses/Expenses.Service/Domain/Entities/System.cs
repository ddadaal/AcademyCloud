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

        public ICollection<OrgTransaction> ReceivedOrgTransactions => Receiver.ReceivedOrgTransactions;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            var time = DateTime.UtcNow;

            var userTransaction = new UserTransaction(Guid.NewGuid(), time, amount, reason, fromUser, ReceiveUser);
            ReceiveUser.ApplyTransaction(userTransaction);

            var orgTransaction = new OrgTransaction(Guid.NewGuid(), time, amount, reason, fromUser.Payer, Receiver, userTransaction);
            ReceivedOrgTransactions.Add(orgTransaction);

            return orgTransaction;
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
