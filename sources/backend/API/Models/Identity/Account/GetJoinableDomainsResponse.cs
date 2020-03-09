using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AcademyCloud.Identity.Protos.Account.GetJoinableDomainsResponse.Types;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class GetJoinableDomainsResponse
    {
        public IEnumerable<JoinableDomain> Domains { get; set; }
    }
}
