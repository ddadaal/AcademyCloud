using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Transactions
{
    public class AccountTransaction
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }

        public decimal Amount { get; set; }

        public TransactionReason Reason { get; set; }
    }
}
