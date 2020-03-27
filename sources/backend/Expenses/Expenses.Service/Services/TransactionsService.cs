using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Transactions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

            // for payed transaction, negate the amount.
            var transactions = user.ReceivedUserTransactions.AsEnumerable().Select(x => x.ToGrpc(false))
                .Concat(user.PayedUserTransactions.AsEnumerable().Select(x => x.ToGrpc(true)))
                .OrderByDescending(x => x.Time)
                .AsEnumerable();

            if (request.Limit > 0)
            {
                transactions = transactions.Take(request.Limit);
            }

            return new GetAccountTransactionsResponse
            {
                Transactions = { transactions }
            };
        }

        public override async Task<GetDomainTransactionsResponse> GetDomainTransactions(GetDomainTransactionsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var domain = user.Domains.FirstOrDefault(x => x.Domain.Id.ToString() == request.DomainId)?.Domain
                ?? throw new RpcException(new Status(StatusCode.PermissionDenied, ""));

            var transactions = domain.ReceivedOrgTransactions.AsEnumerable().Select(x => x.ToGrpc(false))
                .Concat(domain.PayedOrgTransactions.AsEnumerable().Select(x => x.ToGrpc(true)))
                .OrderByDescending(x => x.Time)
                .AsEnumerable();

            if (request.Limit > 0)
            {
                transactions = transactions.Take(request.Limit);
            }

            return new GetDomainTransactionsResponse
            {
                Transactions = { transactions }
            };

        }

        public override async Task<GetProjectTransactionsResponse> GetProjectTransactions(GetProjectTransactionsRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var project = user.Projects
                .Select(x => x.Project)
                .FirstOrDefault(x => x.Id.ToString() == request.ProjectId)
                ?? throw new RpcException(new Status(StatusCode.PermissionDenied, $"Unable to find project ${request.ProjectId} for user ${user.Id}."));


            var transactions = project.ReceivedOrgTransactions.AsEnumerable().Select(x => x.ToGrpc(false))
                .Concat(project.PayedOrgTransactions.AsEnumerable().Select(x => x.ToGrpc(true)))
                .OrderByDescending(x => x.Time)
                .AsEnumerable();

            if (request.Limit > 0)
            {
                transactions = transactions.Take(request.Limit);
            }

            return new GetProjectTransactionsResponse
            {
                Transactions = { transactions }
            };
        }

        public override async Task<GetSystemTransactionsResponse> GetSystemTransactions(GetSystemTransactionsRequest request, ServerCallContext context)
        {
            // let it throw if there is no system instance
            var system = await dbContext.Systems.FirstAsync();

            var transactions = system.ReceivedOrgTransactions.AsQueryable();
            
            if (request.Limit > 0)
            {
                transactions = transactions.Take(request.Limit);
            }

            return new GetSystemTransactionsResponse
            {
                Transactions = { transactions.Select(x => x.ToGrpc(false)) }
            };
        }
    }
}
