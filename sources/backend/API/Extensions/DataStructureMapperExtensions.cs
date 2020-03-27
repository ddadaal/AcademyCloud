using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Expenses.Transactions;

namespace AcademyCloud.API.Extensions
{
    public static class AccountTransactionMapper
    {

        public static Models.Expenses.Transactions.AccountTransaction ToApiModel(this AcademyCloud.Expenses.Protos.Transactions.AccountTransaction grpcModel)
        {
            return new Models.Expenses.Transactions.AccountTransaction
            {
                Id = grpcModel.Id,
                Amount = grpcModel.Amount,
                Reason = grpcModel.Reason,
                Time = grpcModel.Time.ToDateTime(),
            };
        }
    }
}
