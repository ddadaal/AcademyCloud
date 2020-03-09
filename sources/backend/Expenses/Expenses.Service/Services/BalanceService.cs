using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Expenses.Protos.Balance;
using Grpc.Core;

namespace AcademyCloud.Expenses.Services
{
    public class BalanceService: Balance.BalanceBase
    {
        public override async Task<GetBalanceResponse> GetBalance(GetBalanceRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override async Task<ChargeResponse> Charge(ChargeRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
