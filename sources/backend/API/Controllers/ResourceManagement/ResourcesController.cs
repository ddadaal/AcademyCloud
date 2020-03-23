using AcademyCloud.API.Models.ResourceManagement;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Controllers.ResourceManagement
{
    [Route("/resources")]
    [ApiController]
    [Authorize]
    public class ResourcesController : Controller
    {
        private readonly ServiceClientFactory factory;

        public ResourcesController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet("limits")]
        public async Task<GetResourcesLimitResponse> GetLimit()
        {
            var resp = await (await factory.GetExpensesInteropClientAsync())
                .GetQuotaStatusOfCurrentProjectUserAsync(new AcademyCloud.Expenses.Protos.Interop.GetQuotaStatusOfCurrentProjectUserRequest { });

            return new GetResourcesLimitResponse
            {
                Used = resp.Used,
                Allocated = resp.Total,
            };
        }



    }
}
