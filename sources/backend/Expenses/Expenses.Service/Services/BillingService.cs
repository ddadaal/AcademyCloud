using AcademyCloud.Expenses.Protos.Billing;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Services
{
    public class BillingService : Billing.BillingBase
    {
        public override Task<GetCurrentAllocatedBillingResponse> GetCurrentAllocatedBilling(GetCurrentAllocatedBillingRequest request, ServerCallContext context)
        {
            return base.GetCurrentAllocatedBilling(request, context);
        }

        public override Task<GetCurrentAllocatedBillingsResponse> GetCurrentAllocatedBillings(GetCurrentAllocatedBillingsRequest request, ServerCallContext context)
        {
            return base.GetCurrentAllocatedBillings(request, context);
        }

        public override Task<GetCurrentUsedBillingResponse> GetCurrentUsedBilling(GetCurrentUsedBillingRequest request, ServerCallContext context)
        {
            return base.GetCurrentUsedBilling(request, context);
        }

        public override Task<GetCurrentUsedBillingsResponse> GetCurrentUsedBillings(GetCurrentUsedBillingsRequest request, ServerCallContext context)
        {
            return base.GetCurrentUsedBillings(request, context);
        }

        public override Task<GetHistoryAllocatedBillingsResponse> GetHistoryAllocatedBillings(GetHistoryAllocatedBillingsRequest request, ServerCallContext context)
        {
            return base.GetHistoryAllocatedBillings(request, context);
        }

        public override Task<GetHistoryUsedBillingsResponse> GetHistoryUsedBillings(GetHistoryUsedBillingsRequest request, ServerCallContext context)
        {
            return base.GetHistoryUsedBillings(request, context);
        }
    }
}
