using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class User : IPayer
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; } = 0;

        public virtual ICollection<UserProjectAssignment> Projects { get; set; } = new List<UserProjectAssignment>();

        public virtual ICollection<UserDomainAssignment> Domains { get; set; } = new List<UserDomainAssignment>();


        public virtual Payer Payer { get; set; }

        public SubjectType SubjectType => SubjectType.User;

        public virtual ICollection<UserTransaction> ReceivedUserTransactions { get; set; } = new List<UserTransaction>();
        public virtual ICollection<UserTransaction> PayedUserTransactions { get; set; } = new List<UserTransaction>();

        public ICollection<OrgTransaction> PayedOrgTransactions => Payer.PayedOrgTransactions;

        public bool Active { get; set; } = true;

        public User PayUser => this;

        /// <summary>
        /// Apply the balance delta to the user.
        /// </summary>
        /// <param name="transaction">The transaction to apply</param>
        public void ApplyTransaction(UserTransaction transaction)
        {
            if (transaction.Receiver == this)
            {
                Balance += transaction.Amount;

                if (!Active && Balance >= 0)
                {
                    Active = true;
                }
            }
            else if (transaction.Payer == this)
            {
                Balance -= transaction.Amount;
                if (Active && Balance < 0)
                {
                    Active = false;
                }
            }
        }

        public void Charge(decimal amount)
        {
            var transaction = new UserTransaction(Guid.NewGuid(), DateTime.UtcNow, amount, TransactionReason.Charge, null, this);
            ReceivedUserTransactions.Add(transaction);
            ApplyTransaction(transaction);
        }

        public void JoinDomain(Domain domain)
        {
            Domains.Add(new UserDomainAssignment(Guid.NewGuid(), domain, this));

        }

        public OrgTransaction Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time)
        {
            return Payer.Pay(receiver, amount, reason, time);
        }

        public User(Guid id, decimal balance)
        {
            Id = id;
            Balance = balance;
            Payer = new Payer(this);
        }

        public User()
        {
        }
    }
}
