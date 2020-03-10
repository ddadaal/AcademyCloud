using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Transactions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Services
{
    [Authorize]
    public class TransactionsService : Transactions.TransactionsBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;

        public TransactionsService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
        }

        public override async Task<GetAccountTransactionsResponse> GetAccountTransactions(GetAccountTransactionsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var transactions = user.ReceivedUserTransactions
                .Concat(user.PayedUserTransactions)
                .Select(x => x.ToGrpc());

            return new GetAccountTransactionsResponse
            {
                Transactions = { transactions }
            };
        }

        public override async Task<GetDomainTransactionsResponse> GetDomainTransactions(GetDomainTransactionsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var domain = user.Domains.FirstOrDefault(x => x.Id.ToString() == request.DomainId)
                ?? throw new RpcException(new Status(StatusCode.PermissionDenied, ""));

            var transactions = domain.ReceivedOrgTransaction
                .Concat(domain.PayedOrgTransaction)
                .Select(x => x.ToGrpc());

            return new GetDomainTransactionsResponse
            {
                Transactions = { transactions }
            };

        }

        public override async Task<GetProjectTransactionsResponse> GetProjectTransactions(GetProjectTransactionsRequest request, ServerCallContext context)
        {
            return await base.GetProjectTransactions(request, context);
        }

        public override async Task<GetSystemTransactionsResponse> GetSystemTransactions(GetSystemTransactionsRequest request, ServerCallContext context)
        {
            return await base.GetSystemTransactions(request, context);
        }
    }
}
