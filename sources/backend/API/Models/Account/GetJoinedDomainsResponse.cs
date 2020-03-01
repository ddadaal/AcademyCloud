using AcademyCloud.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Account
{
    public class GetJoinedDomainsResponse
    {
        public IEnumerable<UserDomainAssignment> Domains { get; set; }
    };
}
