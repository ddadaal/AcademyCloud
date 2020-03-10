using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Balance;
using Grpc.Core;

namespace AcademyCloud.Expenses.Services
{
    public class BalanceService : Balance.BalanceBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;

        public BalanceService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
        }

        public override async Task<GetBalanceResponse> GetBalance(GetBalanceRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            return new GetBalanceResponse
            {
                Balance = user.Balance,
            };
        }

        public override async Task<ChargeResponse> Charge(ChargeRequest request, ServerCallContext context)
        {
            var tokenClaims = tokenClaimsAccessor.TokenClaims;

            var user = await dbContext.Users.FindIfNullThrowAsync(tokenClaims.UserId);

            var transaction = user.Charge(request.Amount);

            // The transaction added inside charge is set to Modified state
            // which should be Added state
            // it's definitely a bug
            dbContext.Entry(transaction).State = Microsoft.EntityFrameworkCore.EntityState.Added;

            await dbContext.SaveChangesAsync();

            return new ChargeResponse
            {
                Balance = user.Balance,
            };
        }
    }
}
