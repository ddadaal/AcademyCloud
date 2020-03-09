using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class User : IPayer, IReceiver
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<UserProjectAssignment> Projects { get; set; }

        public virtual ICollection<Domain> Domains { get; set; }

        public bool Active { get; set; }

        public SubjectType SubjectType => SubjectType.User;

        public virtual ICollection<UserTransaction> ReceivedUserTransactions { get; set; }
        public virtual ICollection<UserTransaction> PayedUserTransactions { get; set; }

        public bool ApplyTransaction(UserTransaction transaction)
        {
            if (transaction.Receiver == this)
            {
                ReceivedUserTransactions.Add(transaction);
                Balance += transaction.Amount;

                if (!Active && Balance >= 0)
                {
                    Active = true;
                }
            }
            else if (transaction.Payer == this)
            {
                PayedUserTransactions.Add(transaction);
                Balance -= transaction.Amount;
                if (Active && Balance < 0)
                {
                    Active = false;
                }
            }
            return Active;

        }

        public void ChangeResourecs(Project project, Resources resources)
        {

        }

        public void Charge(decimal amount)
        {
            var transaction = new UserTransaction(Guid.NewGuid(), DateTime.UtcNow, amount, new TransactionReason(TransactionType.Charge, ""), null, this);
            ApplyTransaction(transaction);
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
