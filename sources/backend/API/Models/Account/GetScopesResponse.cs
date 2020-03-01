using AcademyCloud.Identity.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Account
{
    public class GetScopesResponse
    {
        public IEnumerable<Scope> Scopes { get; set; }
    }
}
