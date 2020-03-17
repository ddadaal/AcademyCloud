using AcademyCloud.API.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.ResourceManagement
{
    public class GetResourcesLimitResponse
    {
        public Resources Used { get; set; }
        public Resources Allocated { get; set; }
    }
}
