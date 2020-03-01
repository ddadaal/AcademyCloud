using Grpc.Net.Client;
using NConsul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Services;
using Microsoft.AspNetCore.Http;
using Grpc.Core.Interceptors;
using Grpc.Core;

namespace AcademyCloud.API.Utils
{
    public class ServiceClientFactory
    {
        private readonly ConsulClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServiceClientFactory(ConsulClient client, IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;

            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        /// <summary>
        /// Create a insecure channel for simpler development.
        /// Took an whole afternoon trying to get SSL working but no
        /// Believe it's a ASP.NET Core problem
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private async Task<CallInvoker> GetInvoker(string serviceName) {
            var response = await client.Catalog.Service($"{serviceName}-80");
            var service = response.Response.First();
            var channel = GrpcChannel.ForAddress($"http://{service.ServiceAddress}:{service.ServicePort}");
            var invoker = channel.Intercept(new AuthenticatedCallInterceptor(httpContextAccessor));
            return invoker;

        }
        public async Task<Authentication.AuthenticationClient> GetAuthenticationClientAsync()
        {
            return new Authentication.AuthenticationClient(await GetInvoker("identityservice"));
        }

        public async Task<Account.AccountClient> GetAccountClientAsync()
        {
            return new Account.AccountClient(await GetInvoker("identityservice"));
        }
    }
}
