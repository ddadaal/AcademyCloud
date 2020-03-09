using AcademyCloud.Expenses.Protos.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Transactions
{
    public class OrgTransaction
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }

        public decimal Amount { get; set; }

        public string PayerId { get; set; }
        
        public string PayerName { get; set; }

        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }

        public TransactionReason Reason { get; set; }

    }
}
