using AcademyCloud.Identity.Services;
using Grpc.Core;
using Grpc.Net.Client;
using NConsul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Services
{
    public class GrpcClientFactory
    {
        private readonly ConsulClient client;

        public GrpcClientFactory(ConsulClient client)
        {
            this.client = client;
        }

        private async Task<GrpcChannel> GetChannel(string serviceName)
        {
            var response = await client.Catalog.Service(serviceName);
            var service = response.Response.First();
            var address = service.ServiceAddress;
            return GrpcChannel.ForAddress(address);
        }

        public async Task<Authentication.AuthenticationClient> GetAuthenticationClientAsync()
        {
            return new Authentication.AuthenticationClient(await GetChannel("identity"));
        }
    }
}
