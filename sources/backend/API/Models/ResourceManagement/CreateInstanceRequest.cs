using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.ResourceManagement
{
    public class CreateInstanceRequest
    {
        public string Name { get; set; }

        public string FlavorName { get; set; }

        public string ImageName { get; set; }

        public int Volume { get; set; }

    }
}
