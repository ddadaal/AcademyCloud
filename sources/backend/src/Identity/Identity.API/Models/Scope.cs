using AcademyCloud.Identity.Domains.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Models
{
    public class Scope
    {
        public string System { get; set; } = null!;

        public string DomainId { get; set; } = null!;
        public string DomainName { get; set; } = null!;

        public string? ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;
    }
}
