using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class User: IPayer, IReceiver
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<UserProjectAssignment> Projects { get; set; }

        public bool Active { get; set; }

        public SubjectType SubjectType => SubjectType.User;

        public void ChangeResourecs(Project project, Resources resources)
        {

        }

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            throw new NotImplementedException();
        }

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            throw new NotImplementedException();
        }
    }
}
