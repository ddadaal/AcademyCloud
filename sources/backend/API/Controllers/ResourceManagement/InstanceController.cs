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
    public class InstanceController : Controller
    {
        private readonly ServiceClientFactory factory;

        public InstanceController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
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
    }
}
