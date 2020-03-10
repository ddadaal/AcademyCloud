using AcademyCloud.API.Models.Identity.Common;
using System.Collections.Generic;

namespace AcademyCloud.API.Models.Identity.Users
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserForSystem> Users { get; set; }
    }
}