using AcademyCloud.API.Models;
using AcademyCloud.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models
{
    public class ScopesResponse
    {
        public IEnumerable<Scope> Scopes { get; set; }
        public Scope DefaultScope { get; set; }

        public Scope? lastLoginScope { get; set; }

        public ScopesResponse(IEnumerable<Scope> scopes, Scope defaultScope, Scope? lastLoginScope)
        {
            Scopes = scopes;
            DefaultScope = defaultScope;
            this.lastLoginScope = lastLoginScope;
        }
    }
}
