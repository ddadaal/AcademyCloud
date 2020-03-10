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

        public virtual Domain? PayerDomain { get; set; }
        public virtual Project? PayerProject { get; set; }
        public virtual User? PayerUser { get; set; }

        public IPayer Payer => (PayerDomain as IPayer) ?? (PayerProject as IPayer) ?? (PayerUser as IPayer)!;

        public virtual System? ReceiverSystem { get; set; }
        public virtual Domain? ReceiverDomain { get; set; }
        public virtual Project? ReceiverProject { get; set; }

        public IReceiver Receiver => (ReceiverSystem as IReceiver) ?? (ReceiverDomain as IReceiver) ?? (ReceiverProject as IReceiver)!;


        public virtual UserTransaction UserTransaction { get; set; }

        public OrgTransaction(Guid id, DateTime time, decimal amount, TransactionReason reason, IPayer payer, IReceiver receiver, UserTransaction userTransaction)
        {
            Id = id;
            Time = time;
            Amount = amount;
            Reason = reason;
            UserTransaction = userTransaction;

            switch (payer)
            {
                case Domain domain:
                    PayerDomain = domain;
                    break;
                case Project project:
                    PayerProject = project;
                    break;
                case User user:
                    PayerUser = user;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(payer));
            }

            switch (receiver)
            {
                case System system:
                    ReceiverSystem = system;
                    break;
                case Domain domain:
                    ReceiverDomain = domain;
                    break;
                case Project project:
                    ReceiverProject = project;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(receiver));
            }
            
            
        }

        protected OrgTransaction()
        {
        }
    }
}
