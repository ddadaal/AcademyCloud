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
    [Route("/resources/volumes")]
    [ApiController]
    [Authorize]
    public class VolumeController: Controller
    {
        private readonly ServiceClientFactory factory;

        public VolumeController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public async Task<GetVolumesResponse> GetVolumes()
        {
            var resp = await (await factory.GetVolumeServiceClient())
               .GetVolumesAsync(new AcademyCloud.ResourceManagement.Protos.Volume.GetVolumesRequest
               {

               });

            return new GetVolumesResponse
            {
                Volumes = resp.Volumes,
            };
        }
    }
}
