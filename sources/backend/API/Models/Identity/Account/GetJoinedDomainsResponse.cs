using AcademyCloud.Identity.Services;
using AcademyCloud.Identity.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class GetJoinedDomainsResponse
    {
        public IEnumerable<UserDomainAssignment> Domains { get; set; }
    };
}
