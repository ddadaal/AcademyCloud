using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services.Users
{
    public class UsersService : Users.UsersBase
    {
        public override Task<GetAccessibleUsersResponse> GetAccessibleUsers(GetAccessibleUsersRequest request, ServerCallContext context)
        {
            return base.GetAccessibleUsers(request, context);
        }

        public override Task<RemoveUserFromSystemResponse> RemoveUserFromSystem(RemoveUserFromSystemRequest request, ServerCallContext context)
        {
            return base.RemoveUserFromSystem(request, context);
        }
    }
}
