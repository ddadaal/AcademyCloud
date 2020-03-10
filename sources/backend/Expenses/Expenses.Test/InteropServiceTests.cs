using AcademyCloud.Expenses.Services;
using AcademyCloud.Expenses.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Expenses.Test
{
    public class InteropServiceTests: CommonTest
    {
        [Fact]
        public async Task TestGetActivities()
        {
            njuadmin.Active = false;

            await db.SaveChangesAsync();

            var service = new InteropService(MockTokenClaimsAccessor(njuadminnjuTokenClaims), db);
            var resp = await service.GetActivity(new Protos.Interop.GetActivityRequest
            {
                Subjects =
                {
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = nju.Id.ToString(), Type = Protos.Common.SubjectType.Domain },
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = lqproject.Id.ToString(), Type = Protos.Common.SubjectType.Project },
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = njuadmin.Id.ToString(), Type = Protos.Common.SubjectType.User },
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = cjd.Id.ToString(), Type = Protos.Common.SubjectType.User },
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = pku.Id.ToString(), Type = Protos.Common.SubjectType.Domain },
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = fcproject.Id.ToString(), Type = Protos.Common.SubjectType.Project },
                    new Protos.Interop.GetActivityRequest.Types.Subject { Id = fc.Id.ToString(), Type = Protos.Common.SubjectType.User },
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
            }, resp.Activities) ;
                

        }

    }
}
