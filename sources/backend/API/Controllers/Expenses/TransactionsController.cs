using AcademyCloud.API.Models.Expenses.Transactions;
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
        [HttpGet("account")]
        public async Task<TransactionResponse<AccountTransaction>> GetAccountTransactions([FromQuery] int limit = 0)
        {
            throw new NotImplementedException();
        }

        [HttpGet("system")]
        public async Task<TransactionResponse<OrgTransaction>> GetSystemTransactions([FromQuery] int limit = 0)
        {
            throw new NotImplementedException();
        }

        [HttpGet("domain/{domainId}")]
        public async Task<TransactionResponse<OrgTransaction>> GetDomainTransactions([FromRoute] string domainId, [FromQuery] int limit = 0)
        {
            throw new NotImplementedException();
        }

        [HttpGet("project/{projectId}")]
        public async Task<TransactionResponse<OrgTransaction>> GetProjectTransactions([FromRoute] string projectId, [FromQuery] int limit = 0)
        {
            throw new NotImplementedException();
        }
    }
}
