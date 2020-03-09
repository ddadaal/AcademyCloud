using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Account;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class GetJoinedDomainsResponse
    {
        public IEnumerable<UserDomainAssignment> Domains { get; set; }
    };
}
