using AcademyCloud.ResourceManagement.Protos.Volume;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.ResourceManagement
{
    public class GetVolumesResponse
    {
        public IEnumerable<Volume> Volumes { get; set; }
    }
}
