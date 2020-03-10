using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Billing;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Services
{
    [Authorize]
    public class BillingService : Billing.BillingBase
    {
        private TokenClaimsAccessor tokenClaimsAccessor;
        private ExpensesDbContext dbContext;

        public BillingService(TokenClaimsAccessor tokenClaimsAccessor, ExpensesDbContext dbContext)
        {
            this.tokenClaimsAccessor = tokenClaimsAccessor;
            this.dbContext = dbContext;
        }

        public override async Task<GetCurrentAllocatedBillingResponse> GetCurrentAllocatedBilling(GetCurrentAllocatedBillingRequest request, ServerCallContext context)
        {
            return await base.GetCurrentAllocatedBilling(request, context);
        }

        public override async Task<GetCurrentAllocatedBillingsResponse> GetCurrentAllocatedBillings(GetCurrentAllocatedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetCurrentAllocatedBillings(request, context);
        }

        public override async Task<GetCurrentUsedBillingResponse> GetCurrentUsedBilling(GetCurrentUsedBillingRequest request, ServerCallContext context)
        {
            return await base.GetCurrentUsedBilling(request, context);
        }

        public override async Task<GetCurrentUsedBillingsResponse> GetCurrentUsedBillings(GetCurrentUsedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetCurrentUsedBillings(request, context);
        }

        public override async Task<GetHistoryAllocatedBillingsResponse> GetHistoryAllocatedBillings(GetHistoryAllocatedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetHistoryAllocatedBillings(request, context);
        }

        public override async Task<GetHistoryUsedBillingsResponse> GetHistoryUsedBillings(GetHistoryUsedBillingsRequest request, ServerCallContext context)
        {
            return await base.GetHistoryUsedBillings(request, context);
        }
    }
}
