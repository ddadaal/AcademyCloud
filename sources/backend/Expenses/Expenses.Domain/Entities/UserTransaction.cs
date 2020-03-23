using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class UserTransaction
    {
        public Guid Id { get; set; }

        public DateTime Time { get; set; }
        
        public decimal Amount { get; set; }

        public virtual TransactionReason Reason { get; set; }


        /// <summary>
        /// Payer of this transaction.
        /// null means it's a charge.
        /// </summary>
        public virtual User? Payer { get; set; }

        public virtual User Receiver { get; set; }

        public UserTransaction(Guid id, DateTime time, decimal amount, TransactionReason reason, User? payer, User receiver)
        {
            Id = id;
            Time = time;
            Amount = amount;
            Reason = reason;
            Payer = payer;
            Receiver = receiver;
        }

        public UserTransaction() { }
    }
}
