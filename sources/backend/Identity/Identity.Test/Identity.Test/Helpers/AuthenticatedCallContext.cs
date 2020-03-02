using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services.Authentication;
using AcademyCloud.Shared;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Test.Helpers
{
    class AuthenticatedCallContext
    {
        public static async Task<TestServerCallContext> Create(IdentityDbContext context, string username, string password, Predicate<Scope> scopeChooser = null)
        {
            if (scopeChooser == null) { scopeChooser = (scope) => true; }

            var service = new AuthenticationService(context, new JwtSettings());

            var scopesResp = await service.GetScopes(new GetScopesRequest { Username = username, Password = password }, TestServerCallContext.Create());

            var chosenScope = scopesResp.Scopes.First(x => scopeChooser(x));

            var tokenResp = await service.Authenticate(new AuthenticationRequest { Username = username, Password = password, Scope = chosenScope }, TestServerCallContext.Create());

            var metadata = new Metadata
            {
                { "Authorization", $"Bearer {tokenResp.Token}" }
            };

            return TestServerCallContext.Create(metadata);
            
        }
    }
}
