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
        public static User TransformUser(Identity.Protos.Common.User user)
        {
            // Dummy transform the user
            // the active status of the user should be retrieved from expenses 
            return new User { Id = user.Id, Name = user.Name, Username = user.Username, Active = true };
        }

        // Dummy resources
        // Should be retrieved from expenses
        public static Resources DummyResources { get; } = new Resources { CPU = 4, Memory = 128, Storage = 256 };
    }
}
