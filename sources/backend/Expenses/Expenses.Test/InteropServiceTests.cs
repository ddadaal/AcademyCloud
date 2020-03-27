using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Common;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static AcademyCloud.Expenses.Data.ExpensesDbContext;

namespace AcademyCloud.Expenses.Test
{
    public class InteropServiceTests : CommonTest
    {
        private UseCycleConfigurations useConfiguration = new UseCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };
        private BillingCycleConfigurations billingConfiguration = new BillingCycleConfigurations
        {
            CheckCycleMs = 500,
            SettleCycleMs = 1000,
        };


        public InteropService CreateService(TokenClaims? tokenClaims = null)
        {
            if (tokenClaims == null)
            {
                tokenClaims = njuadminnjuTokenClaims;
            }
            var useTask = ConfigureTask<UseCycleTask, UseCycleConfigurations>(useConfiguration);
            var billingTask = ConfigureTask<BillingCycleTask, BillingCycleConfigurations>(billingConfiguration);

            return new InteropService(MockTokenClaimsAccessor(tokenClaims), db, useTask, billingTask);
        }


        [Fact]
        public async Task TestGetActivities()
        {
            var service = CreateService();
            njuadmin.Active = false;

            await db.SaveChangesAsync();

            var resp = await service.GetActivity(new Protos.Interop.GetActivityRequest
            {
                Subjects =
                {
                    new Protos.Interop.Subject { Id = nju.Id.ToString(), Type = Protos.Common.SubjectType.Domain },
                    new Protos.Interop.Subject { Id = lqproject.Id.ToString(), Type = Protos.Common.SubjectType.Project },
                    new Protos.Interop.Subject { Id = njuadmin.Id.ToString(), Type = Protos.Common.SubjectType.User },
                    new Protos.Interop.Subject { Id = cjd.Id.ToString(), Type = Protos.Common.SubjectType.User },
                    new Protos.Interop.Subject { Id = pku.Id.ToString(), Type = Protos.Common.SubjectType.Domain },
                    new Protos.Interop.Subject { Id = fcproject.Id.ToString(), Type = Protos.Common.SubjectType.Project },
                    new Protos.Interop.Subject { Id = fc.Id.ToString(), Type = Protos.Common.SubjectType.User },
                }
            }, TestContext);

            Assert.Equal(new Dictionary<string, bool>
            {
                [nju.Id.ToString()] = false,
                [lqproject.Id.ToString()] = false,
                [njuadmin.Id.ToString()] = false,
                [cjd.Id.ToString()] = true,
                [pku.Id.ToString()] = true,
                [fcproject.Id.ToString()] = true,
                [fc.Id.ToString()] = true,
            }, resp.Activities);

        }

        [Fact]
        public async Task TestGetQuotas()
        {
            var service = CreateService();

            nju.Quota = new Domain.ValueObjects.Resources(10, 0, 0);
            lqproject.Quota = new Domain.ValueObjects.Resources(20, 30, 40);
            await db.SaveChangesAsync();

            var resp = await service.GetQuota(new Protos.Interop.GetQuotaRequest
            {
                Subjects =
                {
                    new Protos.Interop.Subject { Id = nju.Id.ToString(), Type = Protos.Common.SubjectType.Domain },
                    new Protos.Interop.Subject { Id = lqproject.Id.ToString(), Type = Protos.Common.SubjectType.Project },
                    new Protos.Interop.Subject { Id = cjd67project.Id.ToString(), Type = Protos.Common.SubjectType.UserProjectAssignment },
                }
            }, TestContext);

            Assert.Equal(new Dictionary<string, Resources>
            {
                [nju.Id.ToString()] = nju.Quota.ToGrpc(),
                [lqproject.Id.ToString()] = lqproject.Quota.ToGrpc(),
                [cjd67project.Id.ToString()] = cjd67project.Quota.ToGrpc(),
            }, resp.Quotas);
        }

        [Fact]
        public async Task TestGetPayUsers()
        {
            var service = CreateService();

            var resp = await service.GetPayUser(new Protos.Interop.GetPayUserRequest
            {
                Subjects =
                {
                    new Protos.Interop.Subject{Id = SystemId.ToString(), Type = SubjectType.System },
                    new Protos.Interop.Subject{Id = nju.Id.ToString(), Type = SubjectType.Domain},
                    new Protos.Interop.Subject{Id = lqproject.Id.ToString(), Type = SubjectType.Project },
                    new Protos.Interop.Subject { Id = cjd67project.Id.ToString(), Type = SubjectType.UserProjectAssignment}
                }
            }, TestContext);

            Assert.Equal(new Dictionary<string, string>
            {
                [SystemId.ToString()] = db.Systems.First().ReceiveUser.Id.ToString(),
                [nju.Id.ToString()] = njuadmin.Id.ToString(),
                [lqproject.Id.ToString()] = lq.Id.ToString(),
                [cjd67project.Id.ToString()] = cjd.Id.ToString(),
            }, resp.PayUsers);
        }

        [Fact]
        public async Task TestGetSystemQuotaStatus()
        {
            var service = CreateService();

            var systemResourecs = new Domain.ValueObjects.Resources(1000, 1000000, 1000);

            nju.Quota = new Domain.ValueObjects.Resources(20, 30, 40);
            pku.Quota = new Domain.ValueObjects.Resources(30, 40, 50);

            cjd67project.Resources = new Domain.ValueObjects.Resources(1, 2, 3);
            fcfcproject.Resources = new Domain.ValueObjects.Resources(2, 3, 4);

            await db.SaveChangesAsync();

            var resp = await service.GetQuotaStatus(new Protos.Interop.GetQuotaStatusRequest
            {
                Subject = new Protos.Interop.Subject { Type = SubjectType.System, Id = "" }
            }, TestContext);

            Assert.Equal(systemResourecs, resp.Total.FromGrpc());
            Assert.Equal(nju.Quota + pku.Quota, resp.Used.FromGrpc());
        }

        [Fact]
        public async Task TestGetDomainQuotaStatus()
        {
            var service = CreateService();

            nju.Quota = new Domain.ValueObjects.Resources(20, 30, 40);

            lqproject.Quota = new Domain.ValueObjects.Resources(1, 2, 3);

            await db.SaveChangesAsync();

            var resp = await service.GetQuotaStatus(new Protos.Interop.GetQuotaStatusRequest
            {
                Subject = new Protos.Interop.Subject { Type = SubjectType.Domain, Id = nju.Id.ToString() }
            }, TestContext);

            Assert.Equal(nju.Quota, resp.Total.FromGrpc());
            Assert.Equal(lqproject.Quota, resp.Used.FromGrpc());
        }
        [Fact]
        public async Task TestGetProjectQuotaStatus()
        {
            var service = CreateService();

            lqproject.Quota = new Domain.ValueObjects.Resources(10, 20, 30);

            cjd67project.Quota = new Domain.ValueObjects.Resources(1, 2, 3);
            lq67project.Quota = new Domain.ValueObjects.Resources(3, 4, 5);

            await db.SaveChangesAsync();

            var resp = await service.GetQuotaStatus(new Protos.Interop.GetQuotaStatusRequest
            {
                Subject = new Protos.Interop.Subject { Type = SubjectType.Project, Id = lqproject.Id.ToString() }
            }, TestContext);

            Assert.Equal(lqproject.Quota, resp.Total.FromGrpc());
            Assert.Equal(cjd67project.Quota + lq67project.Quota, resp.Used.FromGrpc());
        }

        [Fact]
        public async Task TestGetQuotaStatusOfCurrentProjectUser()
        {
            var service = CreateService(cjdlqTokenClaims);
            cjd67project.Quota = new Domain.ValueObjects.Resources(3, 4, 5);
            cjd67project.Resources = new Domain.ValueObjects.Resources(2, 3, 4);
            lq67project.Quota = new Domain.ValueObjects.Resources(8, 9, 10);

            await db.SaveChangesAsync();

            var resp = await service.GetQuotaStatusOfCurrentProjectUser(new Protos.Interop.GetQuotaStatusOfCurrentProjectUserRequest
            {

            }, TestContext);

            Assert.Equal(cjd67project.Quota, resp.Total.FromGrpc());
            Assert.Equal(cjd67project.Resources, resp.Used.FromGrpc());
        }

        [Fact]
        public async Task TestChangeProjectUserResources()
        {
            var service = CreateService(cjdlqTokenClaims);
            var initial = new Domain.ValueObjects.Resources(3, 4, 5);
            cjd67project.Resources = initial;
            db.UseCycleEntries.Add(new UseCycleEntry(cjd67project.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(lqproject.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(nju.UseCycleSubject));
            await db.SaveChangesAsync();

            // change delta
            var delta = new Domain.ValueObjects.Resources(5, 6, 7);

            await service.ChangeProjectUserResources(new Protos.Interop.ChangeProjectUserResourcesRequest
            {
                ResourcesDelta = delta.ToGrpc()
            }, TestContext);

            // there should be a UseCycleRecord of the initial resources on cjd67project, lqproject and nju domain

            void AssertInitial(UseCycleSubject subject)
            {
                var record = Assert.Single(subject.UseCycleRecords);
                Assert.Equal(initial, record.Resources);
                Assert.Equal(PricePlan.Instance.Calculate(initial), record.Amount);
            }

            AssertInitial(cjd67project.UseCycleSubject);
            AssertInitial(lqproject.UseCycleSubject);
            AssertInitial(nju.UseCycleSubject);
            Assert.Equal(cjd67project.Resources, initial + delta);
            Assert.Equal(lqproject.Resources, initial + delta);
            Assert.Equal(nju.Resources, initial + delta);
        }

        [Fact]
        public async Task TestChangeProjectUserResourcesOnSocialProject()
        {
            var socialDomain = db.Domains.Find(Constants.SocialDomainId);
            var lqsocialproject = new Project(Guid.NewGuid(), lq, socialDomain, Domain.ValueObjects.Resources.QuotaForSocialProject); 
            var lqlqsocialproject = new UserProjectAssignment(Guid.NewGuid(), lq, lqsocialproject, Domain.ValueObjects.Resources.QuotaForSocialProject);
            var lqlqsocialprojectTokenClaims = new TokenClaims(false, true, lq.Id, Constants.SocialDomainId, lqsocialproject.Id, lqlqsocialproject.Id, UserRole.Admin);

            // set this token as a social project token.
            var initial = new Domain.ValueObjects.Resources(3, 4, 5);
            lqlqsocialproject.Resources = initial.Clone();
            db.UseCycleEntries.Add(new UseCycleEntry(lqlqsocialproject.UseCycleSubject));
            db.UseCycleEntries.Add(new UseCycleEntry(lqsocialproject.UseCycleSubject));
            db.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(lqlqsocialproject.BillingCycleSubject));
            db.BillingCycleEntries.Add(new Domain.Entities.BillingCycle.BillingCycleEntry(lqsocialproject.BillingCycleSubject));
            await db.SaveChangesAsync();

            var service = CreateService(lqlqsocialprojectTokenClaims);
            // change delta
            var delta = new Domain.ValueObjects.Resources(-3, -4, -5);

            await service.ChangeProjectUserResources(new Protos.Interop.ChangeProjectUserResourcesRequest
            {
                ResourcesDelta = delta.ToGrpc()
            }, TestContext);

            // no billing is to be allocated to user.
            Assert.Empty(lqlqsocialproject.BillingCycleRecords);

            // No resources is now being used
            Assert.Equal(Domain.ValueObjects.Resources.Zero, lqsocialproject.Resources);

            // lq should pay the resources price
            var transaction = Assert.Single(lq.PayedUserTransactions);
            Assert.Equal(PricePlan.Instance.Calculate(initial), transaction.Amount);




        }

    }
}
