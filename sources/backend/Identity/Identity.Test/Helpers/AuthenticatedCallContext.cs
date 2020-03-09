using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domain.Entities;
using AcademyCloud.Identity.Extensions;
using AcademyCloud.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Authentication;
using AcademyCloud.Identity.Services;

namespace AcademyCloud.Identity.Test.Helpers
{
    public static class AuthenticatedCallContext
    {
        private static async Task<(Scope, string)> GetAuthenticationInfo(IdentityDbContext context, string username, string password, Predicate<Scope>? scopeChooser = null)
        {
            if (scopeChooser == null) { scopeChooser = (scope) => true; }

            // won't use the change scope
            var service = new AuthenticationService(context, new JwtSettings(), null!);

            var scopesResp = await service.GetScopes(new GetScopesRequest { Username = username, Password = password }, TestServerCallContext.Create());

            return (scopesResp.Scopes.First(x => scopeChooser(x)), scopesResp.UserId);
        }

        public static async Task<TokenClaimsAccessor> MockTokenClaimsAccessor(IdentityDbContext context, User user, Predicate<Scope>? scopeChooser = null)
        {
            return await MockTokenClaimsAccessor(context, user.Username, user.Password, scopeChooser);

        }

        public static async Task<TokenClaimsAccessor> MockTokenClaimsAccessor(IdentityDbContext context, string username = "system", string password = "system", Predicate<Scope>? scopeChooser = null)
        {
            var (scope, userId) = await GetAuthenticationInfo(context, username, password, scopeChooser);

            var tokenClaims = new TokenClaims(scope.System, scope.Social, userId, scope.DomainId, scope.ProjectId, (UserRole)scope.Role);

            var mockHttpAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new DefaultHttpContext
            {
                User = tokenClaims.ToClaimsPrincipal()
            };

            mockHttpAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            return new TokenClaimsAccessor(mockHttpAccessor.Object);



        }
    }
}
