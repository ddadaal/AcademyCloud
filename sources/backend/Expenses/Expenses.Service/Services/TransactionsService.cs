using AcademyCloud.Expenses.Protos.Transactions;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Services
{
    public class TransactionsService : Transactions.TransactionsBase
    {
        public override Task<GetAccountTransactionsResponse> GetAccountTransactions(GetAccountTransactionsRequest request, ServerCallContext context)
        {
            return base.GetAccountTransactions(request, context);
        }

        public override Task<GetDomainTransactionsResponse> GetDomainTransactions(GetDomainTransactionsRequest request, ServerCallContext context)
        {
            return base.GetDomainTransactions(request, context);
        }

        public override Task<GetProjectTransactionsResponse> GetProjectTransactions(GetProjectTransactionsRequest request, ServerCallContext context)
        {
            return base.GetProjectTransactions(request, context);
        }

        public override Task<GetSystemTransactionsResponse> GetSystemTransactions(GetSystemTransactionsRequest request, ServerCallContext context)
        {
            return base.GetSystemTransactions(request, context);
        }
    }
}
