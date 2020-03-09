using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public interface IPayer
    {
        bool Pay(IReceiver receiver, decimal amount, TransactionReason reason);

        Guid Id { get; }

        SubjectType SubjectType { get; }

        bool Active { get; }

    }
}
