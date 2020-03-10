using AcademyCloud.API.Extensions;
using AcademyCloud.API.Models.Expenses.Transactions;
using AcademyCloud.API.Utils;
using AcademyCloud.Expenses.Protos.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Controllers.Expenses
{
    [Route("/expenses/transactions")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly ServiceClientFactory factory;

        public TransactionsController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet("account")]
        public async Task<TransactionResponse<AccountTransaction>> GetAccountTransactions([FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetAccountTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetAccountTransactionsRequest { Limit = limit });

            return new TransactionResponse<AccountTransaction>
            {
                Transactions = expensesResp.Transactions.Select(x => x.ToApiModel()),
            };
        }
        
        private async Task<OrgTransaction> ConvertToApiTransaction(AcademyCloud.Expenses.Protos.Transactions.OrgTransaction grpcTransaction)
        {
            return new OrgTransaction
            {
                Id = grpcTransaction.Id,
                Amount = grpcTransaction.Amount,
                PayerId = grpcTransaction.Payer.Id,
                PayerName = grpcTransaction.Payer.Id,
                ReceiverId = grpcTransaction.Receiver.Id,
                ReceiverName = grpcTransaction.Receiver.Id,
                Reason = grpcTransaction.Reason,
                Time = grpcTransaction.Time.ToDateTime(),
            };

        }

        [HttpGet("system")]
        public async Task<TransactionResponse<OrgTransaction>> GetSystemTransactions([FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetSystemTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetSystemTransactionsRequest
                {
                    Limit = limit
                });

            var transformed = expensesResp.Transactions.Select(ConvertToApiTransaction);

            await Task.WhenAll(transformed);

            return new TransactionResponse<OrgTransaction>
            {
                Transactions = transformed.Select(x => x.Result)
            };
        }

        [HttpGet("domain/{domainId}")]
        public async Task<TransactionResponse<OrgTransaction>> GetDomainTransactions([FromRoute] string domainId, [FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetDomainTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetDomainTransactionsRequest
                {
                    Limit = limit
                });

            var transformed = expensesResp.Transactions.Select(ConvertToApiTransaction);

            await Task.WhenAll(transformed);

            return new TransactionResponse<OrgTransaction>
            {
                Transactions = transformed.Select(x => x.Result)
            };
        }

        [HttpGet("project/{projectId}")]
        public async Task<TransactionResponse<OrgTransaction>> GetProjectTransactions([FromRoute] string projectId, [FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetProjectTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetProjectTransactionsRequest
                {
                    Limit = limit
                });

            var transformed = expensesResp.Transactions.Select(ConvertToApiTransaction);

            await Task.WhenAll(transformed);

            return new TransactionResponse<OrgTransaction>
            {
                Transactions = transformed.Select(x => x.Result)
            };
        }
    }
}
