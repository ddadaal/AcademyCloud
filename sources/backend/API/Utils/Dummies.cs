using AcademyCloud.API.Models.Common;
using AcademyCloud.API.Models.Identity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Utils
{
    public static class Dummies
    {
        public static User FromGrpc(Identity.Protos.Common.User user)
        {
            return new User { Id = user.Id, Name = user.Name };
        }

        // Dummy resources
        // Should be retrieved from expenses
        public static Resources DummyResources { get; } = new Resources { Cpu = 4, Memory = 128, Storage = 256 };
    }
}
