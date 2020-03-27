using AcademyCloud.Identity.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AcademyCloud.Identity.Test
{
    public class InteropTests : CommonTest
    {
        protected InteropService service;
        public InteropTests()
        {
            service = new InteropService(db);
        }

        [Fact]
        public async Task TestGetNames()
        {
            var resp = await service.GetNames(new Protos.Interop.GetNamesRequest
            {
                Subjects =
                {
                    new Protos.Interop.GetNamesRequest.Types.Subject { Id = cjd.Id.ToString(), Type = Protos.Interop.GetNamesRequest.Types.SubjectType.User },
                    new Protos.Interop.GetNamesRequest.Types.Subject { Id = cjy.Id.ToString(), Type = Protos.Interop.GetNamesRequest.Types.SubjectType.User },
                    new Protos.Interop.GetNamesRequest.Types.Subject { Id = cjd67project.Id.ToString(), Type = Protos.Interop.GetNamesRequest.Types.SubjectType.UserProjectAssignment },
                    new Protos.Interop.GetNamesRequest.Types.Subject { Id = fcproject.Id.ToString(), Type = Protos.Interop.GetNamesRequest.Types.SubjectType.Project },
                    new Protos.Interop.GetNamesRequest.Types.Subject { Id = nju.Id.ToString(), Type = Protos.Interop.GetNamesRequest.Types.SubjectType.Domain },
                    new Protos.Interop.GetNamesRequest.Types.Subject { Id = "whatever", Type = Protos.Interop.GetNamesRequest.Types.SubjectType.System },
                }
            }, TestContext);

            Assert.Equal(new Dictionary<string, string>
            {
                [cjd.Id.ToString()] = cjd.Name,
                [cjy.Id.ToString()] = cjy.Name,
                [cjd67project.Id.ToString()] = cjd.Name,
                [fcproject.Id.ToString()] = fcproject.Name,
                [nju.Id.ToString()] = nju.Name,
                ["whatever"] = "system",
            }, resp.IdNameMap);

        }
    }
}
