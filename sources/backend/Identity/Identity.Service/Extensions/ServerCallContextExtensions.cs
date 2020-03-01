using AcademyCloud.Shared;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Extensions
{
    public static class ServerCallContextExtensions
    {
        public static TokenClaims GetTokenClaims(this ServerCallContext context)
        {
            var claims = context.GetHttpContext().User;

            var tokenClaims = TokenClaims.FromClaimPrincinpal(claims);

            return tokenClaims;
        }
    }
}
