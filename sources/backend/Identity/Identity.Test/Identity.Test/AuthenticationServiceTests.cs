using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services;
using AcademyCloud.Identity.Services.Authentication;
using AcademyCloud.Shared;
using Grpc.Core;
using Identity.Test.DbContext;
using Identity.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Identity.Test
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        public JwtSettings jwtSettings = new JwtSettings();

        private readonly DbContextOptions<IdentityDbContext> options =
            new DbContextOptionsBuilder<IdentityDbContext>()
                    .UseInMemoryDatabase(databaseName: "Identity Database")
                    .Options;

        [TestMethod]
        public async Task TestGetScopesSystem1()
        {
            // Arrange
            using var context = new IdentityDbContext(options);
            await MockDbContext.FillDataAsync(context);
            var service = new AuthenticationService(context, jwtSettings);

            // Act
            var resp = await service.GetScopes(new GetScopesRequest() { Username = "system", Password = "system" }, TestServerCallContext.Create());

            // Assert
            Assert.IsTrue(resp.Success);
        }
    }
}
