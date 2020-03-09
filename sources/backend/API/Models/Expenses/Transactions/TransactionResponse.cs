using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Transactions
{
    public class TransactionResponse<T>
    {
        public IEnumerable<T> Transactions { get; set; }
    }
}
