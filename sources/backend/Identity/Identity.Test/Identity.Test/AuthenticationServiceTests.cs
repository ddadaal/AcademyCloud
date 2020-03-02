using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services;
using AcademyCloud.Identity.Services.Authentication;
using AcademyCloud.Shared;
using Grpc.Core;
using Identity.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Identity.Test
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        [TestMethod]
        public async Task TestGetScopesSystem1()
        {
            // Arrange
            using var context = MockDbContext.GetDbContext();
            var service = new AuthenticationService(context, new JwtSettings());

            // Act
            var resp = await service.GetScopes(new GetScopesRequest() { Username = "system", Password = "system" }, TestServerCallContext.Create());

            // Assert
            Assert.IsTrue(resp.Success);
            Assert.IsTrue(resp.Scopes[0].System);
        }
    }
}
