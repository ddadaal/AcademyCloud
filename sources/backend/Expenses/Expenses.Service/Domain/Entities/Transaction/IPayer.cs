using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.Transaction
{
    public interface IPayer
    {

        Guid Id { get; }

        SubjectType SubjectType { get; }

        bool Active { get; }

        ICollection<OrgTransaction> PayedOrgTransactions { get; }

        User PayUser { get; }

        bool Pay(IReceiver receiver, decimal amount, TransactionReason reason, DateTime time);
    }
}
