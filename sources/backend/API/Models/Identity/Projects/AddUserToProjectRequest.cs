
using AcademyCloud.Shared;

namespace AcademyCloud.API.Models.Identity.Projects
{
    public class AddUserToProjectRequest
    {
        public string UserId { get; set; }

        public UserRole Role { get; set; }
    }
}