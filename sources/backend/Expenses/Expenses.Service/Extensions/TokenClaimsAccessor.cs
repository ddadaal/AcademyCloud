using AcademyCloud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Extensions
{
    public class TokenClaimsAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenClaimsAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public TokenClaims TokenClaims => httpContextAccessor.HttpContext.User.ToTokenClaims();

    }

}
