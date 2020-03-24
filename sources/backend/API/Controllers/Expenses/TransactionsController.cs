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

        private async Task<IEnumerable<OrgTransaction>> ConvertToApiModels(IEnumerable<AcademyCloud.Expenses.Protos.Transactions.OrgTransaction> input)
        {
            // Pick all ids and types
            var subjects = input.Select(x => x.Payer)
                .Concat(input.Select(x => x.Receiver))
                .Select(x => new AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.Subject
                {
                    Id = x.Id,
                    Type = (AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.SubjectType)x.Type,
                });

            // Send to Identity service to check their names
            var nameMapResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new AcademyCloud.Identity.Protos.Interop.GetNamesRequest
                {
                    Subjects = { subjects }
                });

            var nameMap = nameMapResp.IdNameMap;

            // Set their names to their respective transaction
            return input.Select(x => new OrgTransaction
            {
                Id = x.Id,
                Amount = x.Amount,
                PayerId = x.Payer.Id,
                PayerName = nameMap[x.Payer.Id],
                ReceiverId = x.Receiver.Id,
                ReceiverName = nameMap[x.Receiver.Id],
                Reason = TransactionReason.FromGrpc(x.Reason),
                Time = x.Time.ToDateTime(),
            });
        }

        [HttpGet("system")]
        public async Task<TransactionResponse<OrgTransaction>> GetSystemTransactions([FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetSystemTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetSystemTransactionsRequest
                {
                    Limit = limit
                });

            var converted = await ConvertToApiModels(expensesResp.Transactions);


            return new TransactionResponse<OrgTransaction>
            {
                Transactions = converted,
            };
        }

        [HttpGet("domain/{domainId}")]
        public async Task<TransactionResponse<OrgTransaction>> GetDomainTransactions([FromRoute] string domainId, [FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetDomainTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetDomainTransactionsRequest
                {
                    Limit = limit,
                    DomainId = domainId,
                });

            var converted = await ConvertToApiModels(expensesResp.Transactions);

            return new TransactionResponse<OrgTransaction>
            {
                Transactions = converted,
            };
        }

        [HttpGet("project/{projectId}")]
        public async Task<TransactionResponse<OrgTransaction>> GetProjectTransactions([FromRoute] string projectId, [FromQuery] int limit = 0)
        {
            var expensesResp = await (await factory.GetTransactionsClient())
                .GetProjectTransactionsAsync(new AcademyCloud.Expenses.Protos.Transactions.GetProjectTransactionsRequest
                {
                    Limit = limit,
                    ProjectId = projectId,
                });

            var converted = await ConvertToApiModels(expensesResp.Transactions);

            return new TransactionResponse<OrgTransaction>
            {
                Transactions = converted,
            };
        }
    }
}
