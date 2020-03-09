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

        public virtual Subject Payer { get; set; }

        public virtual Subject Receiver { get; set; }

        public virtual UserTransaction UserTransaction { get; set; }

    }
}
