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
        Guid Id { get; }

        SubjectType SubjectType { get; }

        Resources Quota { get; }
        IReceiver BillingReceiver { get; }

        void Settle(PricePlan plan, DateTime lastSettled, DateTime now);
    }
}
