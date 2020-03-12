using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities.Transaction
{
    public interface IReceiver
    {

        Guid Id { get; }

        SubjectType SubjectType { get; }

        ICollection<OrgTransaction> ReceivedOrgTransactions { get; }

        User ReceiveUser { get; }

        OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason, DateTime time);
    }
}
