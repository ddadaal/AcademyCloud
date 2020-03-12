using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Interop;

namespace AcademyCloud.API.Utils
{
    public class InteropServices
    {
        private readonly ServiceClientFactory factory;

        public InteropServices(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Get names.
        /// </summary>
        /// <param name="names">Tuple (SubjectType, id)</param>
        /// <returns>Map from id to name</returns>
        public async Task<IDictionary<string, string>> GetNamesAsync(params (GetNamesRequest.Types.SubjectType, string)[] names)
        {
            var subjects = names.Select((i) => new GetNamesRequest.Types.Subject() { Type = i.Item1, Id = i.Item2 });

            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest { Subjects = { subjects } });

            return namesResp.IdNameMap;

        }
    }
}
