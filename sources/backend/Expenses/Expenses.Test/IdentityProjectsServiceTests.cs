using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static AcademyCloud.Shared.Constants;

namespace AcademyCloud.Expenses.Test
{
    public class IdentityProjectsServiceTests: CommonTest
    {
        private BillingCycleConfigurations billingConfiguration = new BillingCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };
        private UseCycleConfigurations useConfiguration = new UseCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };

        public (IdentityService, BillingCycleTask, UseCycleTask) CreateService(TokenClaims? tokenClaims = null)
        {
            var billingTask = ConfigureTask<BillingCycleTask, BillingCycleConfigurations>(billingConfiguration);
            var useTask = ConfigureTask<UseCycleTask, UseCycleConfigurations>(useConfiguration);
            var claimsAccessor = MockTokenClaimsAccessor(tokenClaims ?? njuadminnjuTokenClaims);

            return (new IdentityService(claimsAccessor, db, useTask, billingTask), billingTask, useTask);
        }
        private Guid projectId = Guid.NewGuid();
        private Guid payUserAssignmentId = Guid.NewGuid();

        private IdentityService service;

        public IdentityProjectsServiceTests()
        {
            var (service, _, _) = CreateService();
            this.service = service;
            service.AddProject(new Protos.Identity.AddProjectRequest
            {
                Id = projectId.ToString(),
                PayUserId = cjd.Id.ToString(),
                PayUserAssignmentId = payUserAssignmentId.ToString(),
            }, TestContext).Wait();
        }

        [Fact]
        public async Task TestAddProject()
        {
            Assert.Equal(new[] { projectId, payUserAssignmentId }.ToList(), db.BillingCycleEntries.Select(x => x.Id).ToList());
            Assert.Equal(new[] { projectId, payUserAssignmentId }.ToList(), db.UseCycleEntries.Select(x => x.Id).ToList());
            var project = db.BillingCycleEntries.Find(projectId).Subject.Project;
            Assert.NotNull(project);
            Assert.Single(project.Users);
            Assert.Equal(cjd, project.PayUser);
        }
    }
}
