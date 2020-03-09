using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public interface IBillingCycleSubject
    {
        Resources Quota { get; }

        void Settle(Resources quota, decimal price);
    }
}
