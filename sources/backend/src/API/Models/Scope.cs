using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models
{
    public class Scope
    {
        public string DomainId { get; set; } = "";
        public string DomainName { get; set; } = "";

        public string? ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;
    }
}
