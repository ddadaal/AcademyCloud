using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
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
    public class IdentityUserService : CommonTest
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

        [Fact]
        public async Task TestRegister()
        {
            var (service, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            await service.AddUser(new Protos.Identity.AddUserRequest
            {
                UserId = userId.ToString(),
                SocialProjectId = projectId.ToString(),
                SocialDomainAssignmentId = Guid.NewGuid().ToString(),
                SocialProjectAssignmentId = Guid.NewGuid().ToString()
            }, TestContext);

            var user = db.Users.Find(userId);
            var project = db.Projects.Find(projectId);
            Assert.NotNull(user);
            Assert.NotNull(project);
            Assert.Equal(user, project.PayUser);


            var userAssignment = Assert.Single(project.Users);
            AssertIEnumerableIgnoreOrder(new[] { user.Payer, project.Payer }, db.ManagementFeeEntries.Select(x => x.Payer));
            AssertIEnumerableIgnoreOrder(new[] { userAssignment.BillingCycleSubject, project.BillingCycleSubject }, db.BillingCycleEntries.Select(x => x.Subject));
            AssertIEnumerableIgnoreOrder(new[] { userAssignment.UseCycleSubject.Id, project.UseCycleSubject.Id, SocialDomainId }, db.UseCycleEntries.Select(x => x.Subject.Id));

            var socialProject = Assert.Single(db.Domains.Find(SocialDomainId).Projects);
            Assert.Equal(socialProject, project);
        }

        [Fact]
        public async Task TestRemoveUser()
        {
            var (service, _, _) = CreateService();

            await service.DeleteUser(new Protos.Identity.DeleteUserRequest
            {
                UserId = cjd.Id.ToString(),
            }, TestContext);

            Assert.Null(db.Users.Find(cjd.Id));

            Assert.Equal(0, lqproject.Users.Count(x => x.User.Id == cjd.Id));
        }
    }
}
