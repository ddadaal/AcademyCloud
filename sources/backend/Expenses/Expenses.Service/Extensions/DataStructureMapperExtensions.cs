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
                PayerId = transaction.Payer.Id.ToString(),
                ReceiverId = transaction.Receiver.Id.ToString(),
            };
        }
    }
}
