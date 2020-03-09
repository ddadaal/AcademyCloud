using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Expenses.Balance;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.API.Controllers.Expenses
{
    [Route("/expenses/balance")]
    [ApiController]
    [Authorize]
    public class BalanceController: Controller
    {

        private readonly ServiceClientFactory factory;

        public BalanceController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public async Task<GetBalanceResponse> GetBalance()
        {
            var resp = await (await factory.GetBalanceClient())
                .GetBalanceAsync(new AcademyCloud.Expenses.Protos.Balance.GetBalanceRequest { });

            return new GetBalanceResponse
            {
                Balance = resp.Balance,
            };
        }

        [HttpPost]
        public async Task<ChargeResponse> Charge([FromBody] ChargeRequest request)
        {
            var resp = await (await factory.GetBalanceClient())
                .ChargeAsync(new AcademyCloud.Expenses.Protos.Balance.ChargeRequest
                {
                    Amount = request.Amount,
                });

            return new ChargeResponse
            {
                Balance = resp.Balance,
            };
        }
    }
}
