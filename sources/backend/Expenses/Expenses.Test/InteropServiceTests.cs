using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Protos.Common;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
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
        private InteropService service;

        public InteropServiceTests()
        {
            service = new InteropService(MockTokenClaimsAccessor(njuadminnjuTokenClaims), db);
        }


        [Fact]
        public async Task TestGetActivities()
        {
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

    }
}
