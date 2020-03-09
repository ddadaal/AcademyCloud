using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public interface IUseCycleSubject
    {
        Resources Resources { get; }

        void Settle(Resources resources, decimal price);
    }
}
