using Grpc.Net.Client;
using NConsul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Services;

namespace AcademyCloud.API.Utils
{
    public class ServiceClientFactory
    {
        private readonly ConsulClient client;

        public ServiceClientFactory(ConsulClient client)
        {
            this.client = client;

            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        private async Task<GrpcChannel> GetChannel(string serviceName)
        {
            var response = await client.Catalog.Service($"{serviceName}-80");
            var service = response.Response.First();
            return GrpcChannel.ForAddress($"http://{service.ServiceAddress}:{service.ServicePort}");
        }

        public async Task<Greeter.GreeterClient> GetGreeterClientAsync()
        {
            return new Greeter.GreeterClient(await GetChannel("identityservice"));
        }


        //public async Task<Authentication.AuthenticationClient> GetAuthenticationClientAsync()
        //{
        //    return new Authentication.AuthenticationClient(await GetChannel("identityapi"));
        //}

        //public async Task<Account.AccountClient> GetAccountClientAsync()
        //{
        //    return new Account.AccountClient(await GetChannel("identityapi"));
        //}
    }
}
