using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.API.Models.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademyCloud.API.Controllers.Expenses
{
    [Route("/expenses/balance")]
    [ApiController]
    [Authorize]
    public class BalanceController: Controller
    {
        [HttpGet]
        public async Task<GetBalanceResponse> GetBalance()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ChargeResponse> Charge([FromBody] ChargeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
