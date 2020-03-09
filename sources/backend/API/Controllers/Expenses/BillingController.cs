using AcademyCloud.API.Models.Expenses.Billing;
using AcademyCloud.Expenses.Protos.Billing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Controllers.Expenses
{
    [Route("/expenses/billing")]
    [ApiController]
    public class BillingController : Controller
    {
        [HttpGet("allocated/current/{subjectType}")]
        public async Task<BillingsResponse<CurrentAllocatedBilling>> GetCurrentAllocatedBillings([FromRoute] BillSubjectType subjectType)
        {

            throw new NotImplementedException();
        }

        [HttpGet("allocated/current/{subjectType}/{id}")]
        public async Task<BillingResponse<CurrentAllocatedBilling>> GetCurrentAllocatedBilling([FromRoute] BillSubjectType subjectType, [FromRoute] string id)
        {

            throw new NotImplementedException();
        }

        [HttpGet("used/current/{subjectType}")]
        public async Task<BillingsResponse<CurrentUsedBilling>> GetCurrentUsedBillings([FromRoute] BillSubjectType subjectType)
        {

            throw new NotImplementedException();
        }

        [HttpGet("used/current/{subjectType}/{id}")]
        public async Task<BillingResponse<CurrentUsedBilling>> GetCurrentUsedBilling([FromRoute] BillSubjectType subjectType, [FromRoute] string id)
        {

            throw new NotImplementedException();
        }

        [HttpGet("allocated/history/{subjectType}/{id}")]
        public async Task<BillingsResponse<HistoryAllocatedBilling>> GetHistoryAllocatedBillings([FromRoute] BillSubjectType subjectType, [FromRoute] string id)
        {

            throw new NotImplementedException();
        }

        [HttpGet("used/history/{subjectType}/{id}")]
        public async Task<BillingsResponse<HistoryUsedBilling>> GetHistoryUsedBillings([FromRoute] BillSubjectType subjectType, [FromRoute] string id)
        {

            throw new NotImplementedException();
        }
    }
}
