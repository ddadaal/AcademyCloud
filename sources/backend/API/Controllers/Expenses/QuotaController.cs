using AcademyCloud.API.Models.Expenses;
using AcademyCloud.API.Utils;
using AcademyCloud.Expenses.Protos.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Controllers.Expenses
{
    [Route("/expenses/quotaStatus")]
    [ApiController]
    public class QuotaController: Controller
    {
        private readonly ServiceClientFactory factory;

        public QuotaController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet("{subjectType}/{id}")]
        public async Task<GetQuotaStatusResponse> GetQuotaStatus([FromRoute] SubjectType subjectType, [FromRoute] string id)
        {
            var resp = await (await factory.GetExpensesInteropClientAsync())
                .GetQuotaStatusAsync(new AcademyCloud.Expenses.Protos.Interop.GetQuotaStatusRequest
                {
                    Subject = new AcademyCloud.Expenses.Protos.Interop.Subject
                    {
                        Id = id,
                        Type = subjectType
                    }
                });

            return new GetQuotaStatusResponse
            {
                Total = resp.Total,
                Used = resp.Used,
            };

        }
    }
}
