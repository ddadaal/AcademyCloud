using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Extensions
{
    public static class TransactionMapper
    {

        public static Protos.Transactions.TransactionReason ToGrpc(this Domain.ValueObjects.TransactionReason reason)
        {
            return new Protos.Transactions.TransactionReason
            {
                Type = (Protos.Transactions.TransactionType)reason.Type,
                Info = reason.Info,
            };
        }

        public static Protos.Transactions.AccountTransaction ToGrpc(this Domain.Entities.UserTransaction transaction)
        {
            return new Protos.Transactions.AccountTransaction
            {
                Id = transaction.Id.ToString(),
                Amount = transaction.Amount,
                Reason = transaction.Reason.ToGrpc(),
                Time = Timestamp.FromDateTime(transaction.Time),
            };
        }
        public static Protos.Transactions.OrgTransaction ToGrpc(this Domain.Entities.OrgTransaction transaction)
        {
            return new Protos.Transactions.OrgTransaction
            {
                Id = transaction.Id.ToString(),
                Amount = transaction.Amount,
                Reason = transaction.Reason.ToGrpc(),
                Time = Timestamp.FromDateTime(transaction.Time),
                Payer = new Protos.Transactions.OrgTransactionSubject
                {
                    Id = transaction.Payer.Id.ToString(),
                    Type = (Protos.Common.SubjectType)transaction.Payer.SubjectType,
                    UserId = transaction.UserTransaction.Payer!.Id.ToString(),
                },
                Receiver = new Protos.Transactions.OrgTransactionSubject
                {
                    Id = transaction.Receiver.Id.ToString(),
                    Type = (Protos.Common.SubjectType)transaction.Receiver.SubjectType,
                    UserId = transaction.UserTransaction.Receiver!.Id.ToString(),
                }
            };
        }

        public static Protos.Billing.Resources ToGrpc(this Domain.ValueObjects.Resources resources)
        {
            return new Protos.Billing.Resources { Cpu = resources.Cpu, Memory = resources.Memory, Storage = resources.Storage };
        }

        public static Protos.Common.SubjectType ToGrpc(this Domain.ValueObjects.SubjectType type)
        {
            return (Protos.Common.SubjectType)type;
        }

    }

}
