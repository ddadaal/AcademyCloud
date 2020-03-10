using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class System : IReceiver
    {
        public Guid Id { get; set; }

        public virtual ICollection<Domain> Domains { get; set; } = new List<Domain>();

        public virtual ICollection<User> SystemUsers { get; set; } = new List<User>();

        public virtual ICollection<OrgTransaction> ReceivedOrgTransactions { get; set; } = new List<OrgTransaction>();

        public virtual User SystemReceiver { get; set; }

        public SubjectType SubjectType => SubjectType.System;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            throw new NotImplementedException();
        }

        public System(Guid id, User systemReceiver)
        {
            Id = id;
            SystemReceiver = systemReceiver;
        }

        public System()
        {
        }
    }
}
