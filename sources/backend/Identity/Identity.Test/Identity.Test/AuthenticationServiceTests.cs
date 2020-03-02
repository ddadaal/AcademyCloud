using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services;
using AcademyCloud.Identity.Services.Authentication;
using AcademyCloud.Shared;
using Grpc.Core;
using Identity.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Test
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async Task TestGetScopesSystem1()
        {
            // Arrange
            using var context = MockDbContext.GetDbContext();
            var service = new AuthenticationService(context, new JwtSettings());

            // Act
            var resp = await service.GetScopes(new GetScopesRequest() { Username = "system", Password = "system" }, TestServerCallContext.Create());

            // Assert
            Assert.True(resp.Success);
            Assert.True(resp.Scopes[0].System);
        }
    }
}
