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
    [Route("/resources/instances")]
    [ApiController]
    [Authorize]
    public class InstanceController: Controller
    {
        private readonly ServiceClientFactory factory;

        public InstanceController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public async Task<ActionResult<GetInstancesResponse>> GetInstances()
        {
            var resp = await (await factory.GetInstanceServiceClient())
               .GetInstancesAsync(new AcademyCloud.ResourceManagement.Protos.Instance.GetInstancesRequest
               {

               });

            return new GetInstancesResponse
            {
                Instances = resp.Instances,
            };

        }
    }
}
