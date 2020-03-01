using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AcademyCloud.Identity.Services.GetJoinableDomainsResponse.Types;

namespace AcademyCloud.API.Models.Account
{
    public class GetJoinableDomainsResponse
    {
        public IEnumerable<JoinableDomain> Domains { get; set; }
    }
}
