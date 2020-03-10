using AcademyCloud.API.Models.Identity.Common;
using System.Collections.Generic;

namespace AcademyCloud.API.Models.Identity.Users
{
    public class GetAccessibleUsersResponse
    {
        public IEnumerable<User> Users { get; set; }
    }
}