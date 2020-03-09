using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.ValueObjects
{
    public class TransactionReason: ValueObject
    {
        public TransactionType Type { get; set; }

        public string Info { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Info;
        }
    }
}
