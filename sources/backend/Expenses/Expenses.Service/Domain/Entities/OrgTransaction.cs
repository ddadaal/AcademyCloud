using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
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
        public virtual User? ReceiverUser { get; set; }
        public IReceiver Receiver => (ReceiverSystem as IReceiver) ?? (ReceiverDomain as IReceiver) ?? (ReceiverProject as IReceiver) ?? (ReceiverUser as IReceiver)!;


        public virtual UserTransaction UserTransaction { get; set; }

    }
}
