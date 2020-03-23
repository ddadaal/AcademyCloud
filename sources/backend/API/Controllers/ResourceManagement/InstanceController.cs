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
    public class InstanceController : Controller
    {
        private readonly ServiceClientFactory factory;

        public InstanceController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet("instances")]
        public async Task<GetInstancesResponse> GetInstances()
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

        [HttpGet("flavors")]
        public async Task<GetFlavorsResponse> GetFlavors()
        {
            var resp = await (await factory.GetInstanceServiceClient())
                .GetFlavorsAsync(new AcademyCloud.ResourceManagement.Protos.Instance.GetFlavorsRequest
                {

                });

            return new GetFlavorsResponse
            {
                Flavors = resp.Flavors
            };

        }

        [HttpGet("images")]
        public async Task<GetImagesResponse> GetImages()
        {
            var resp = await (await factory.GetInstanceServiceClient())
                .GetImagesAsync(new AcademyCloud.ResourceManagement.Protos.Instance.GetImagesRequest
                {

                });

            return new GetImagesResponse
            {
                Images = resp.Images
            };
        }

        [HttpPost("instances")]
        public async Task<ActionResult> CreateInstance([FromBody] CreateInstanceRequest request)
        {
            var resp = await (await factory.GetInstanceServiceClient())
                .CreateInstanceAsync(new AcademyCloud.ResourceManagement.Protos.Instance.CreateInstanceRequest
                {
                    Name = request.Name,
                    ImageName = request.ImageName,
                    FlavorName = request.FlavorName,
                    Volume = request.Volume
                });

            // add the resources usage to expenses
            await (await factory.GetExpensesInteropClientAsync())
               .ChangeProjectUserResourcesAsync(new AcademyCloud.Expenses.Protos.Interop.ChangeProjectUserResourcesRequest
               {
                   ResourcesDelta = new AcademyCloud.Expenses.Protos.Common.Resources
                   {
                       Cpu = resp.Flavor.Cpu,
                       Memory = resp.Flavor.Memory,
                       Storage = request.Volume,
                   }
               });

            return Created(resp.InstanceId, resp.InstanceId);

        }

        [HttpDelete("instances/{instanceId}")]
        public async Task<ActionResult> DeleteInstance([FromRoute] string instanceId)
        {
            var resp = await (await factory.GetInstanceServiceClient())
                .DeleteInstanceAsync(new AcademyCloud.ResourceManagement.Protos.Instance.DeleteInstanceRequest
                {
                    InstanceId = instanceId,
                });

            // remove the resources usage to expenses
            await (await factory.GetExpensesInteropClientAsync())
               .ChangeProjectUserResourcesAsync(new AcademyCloud.Expenses.Protos.Interop.ChangeProjectUserResourcesRequest
               {
                   ResourcesDelta = new AcademyCloud.Expenses.Protos.Common.Resources
                   {
                       Cpu = -resp.Flavor.Cpu,
                       Memory = -resp.Flavor.Memory,
                       Storage = -resp.Volume,
                   }
               });

            return NoContent();
        }

        [HttpGet("instances/{instanceId}/start")]
        public async Task<ActionResult> StartInstance([FromRoute] string instanceId)
        {
            await (await factory.GetInstanceServiceClient())
                .StartInstanceAsync(new AcademyCloud.ResourceManagement.Protos.Instance.StartInstanceRequest
                {
                    InstanceId = instanceId
                });

            return NoContent();
        }

        [HttpGet("instances/{instanceId}/reboot")]
        public async Task<ActionResult> RebootInstance([FromRoute] string instanceId, [FromQuery] bool hard)
        {
            await (await factory.GetInstanceServiceClient())
                .RebootInstanceAsync(new AcademyCloud.ResourceManagement.Protos.Instance.RebootInstanceRequest
                {
                    InstanceId = instanceId,
                    Hard = hard
                });

            return NoContent();
        }

        [HttpGet("instances/{instanceId}/stop")]
        public async Task<ActionResult> StopInstance([FromRoute] string instanceId)
        {
            await (await factory.GetInstanceServiceClient())
                .StopInstanceAsync(new AcademyCloud.ResourceManagement.Protos.Instance.StopInstanceRequest
                {
                    InstanceId = instanceId
                });

            return NoContent();
        }
    }
}
