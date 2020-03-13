using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.UseCycle
{
    public interface IUseCycleSubject
    {
        Guid Id { get; }

        SubjectType SubjectType { get; }

        Resources Resources { get; }

        bool Settle(decimal price, DateTime lastSettled, DateTime now);
    }
}
