using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.BillingCycle
{
    public interface IBillingCycleSubject : IPayer
    {

        Resources Quota { get; }
        IReceiver BillingReceiver { get; }

        /// <summary>
        /// Try settle. If quota is Zero, it will not be settled.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="lastSettled"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        bool Settle(decimal price, DateTime lastSettled, DateTime now, TransactionReason reson);
    }
}
