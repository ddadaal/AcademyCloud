using AcademyCloud.Shared;

namespace AcademyCloud.API.Controllers.Identity
{
    public class ChangeUserRoleRequest
    {
        public UserRole Role { get; set; }
    }
}