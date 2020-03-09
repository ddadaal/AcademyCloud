using AcademyCloud.Identity.Protos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class GetScopesResponse
    {
        public IEnumerable<Scope> Scopes { get; set; }
    }
}
