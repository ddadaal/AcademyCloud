using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services;
using AcademyCloud.Identity.Test.Helpers;
using AcademyCloud.Shared;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Authentication;
using Xunit;
using static AcademyCloud.Identity.Test.Helpers.AuthenticatedCallContext;

namespace AcademyCloud.Identity.Test
{
    public class AuthenticationServiceTests : CommonTest
    {
        [Fact]
        public async Task TestGetScopesSystem1()
        {
            // Arrange
            var service = new AuthenticationService(db, new JwtSettings(), await MockTokenClaimsAccessor(db, "system", "system"));

            // Act
            var resp = await service.GetScopes(new GetScopesRequest() { Username = "system", Password = "system" }, TestServerCallContext.Create());

            // Assert
            Assert.True(resp.Success);
            Assert.True(resp.Scopes[0].System);
        }
    }
}
