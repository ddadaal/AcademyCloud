using System.Collections.Generic;

namespace AcademyCloud.API.Models.Identity.Projects
{
    public class GetAccessibleProjectsResponse
    {
        public IEnumerable<Project> Projects { get; set; }
    }
}