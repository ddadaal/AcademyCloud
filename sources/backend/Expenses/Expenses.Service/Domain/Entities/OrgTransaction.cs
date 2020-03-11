using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    /// <summary>
    /// Represents a transaction from organization or user (user only pays management fee) to organization
    /// Receiver cannot be UserProjectAssignment since User doesn't pay for their resources use
    /// </summary>
    public class OrgTransaction
    {
        public Guid Id { get; set; }

        public DateTime Time { get; set; }

        public decimal Amount { get; set; }

        public virtual TransactionReason Reason { get; set; }

        public virtual Payer Payer { get; set; }

        public virtual Receiver Receiver { get; set; }

        public virtual UserTransaction UserTransaction { get; set; }

        public OrgTransaction(Guid id, DateTime time, decimal amount, TransactionReason reason, Payer payer, Receiver receiver, UserTransaction userTransaction)
        {
            Id = id;
            Time = time;
            Amount = amount;
            Reason = reason;
            UserTransaction = userTransaction;
            Payer = payer;
            Receiver = receiver;
        }

        protected OrgTransaction()
        {
        }
    }
}
