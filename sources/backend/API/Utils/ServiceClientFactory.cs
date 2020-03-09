using Grpc.Net.Client;
using NConsul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Account;
using AcademyCloud.Identity.Protos.Authentication;
using AcademyCloud.Identity.Protos.Domains;
using AcademyCloud.Identity.Protos.Projects;
using AcademyCloud.Identity.Protos.Users;
using Microsoft.AspNetCore.Http;
using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Primitives;
using AcademyCloud.Expenses.Protos.Balance;

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
        private async Task<CallInvoker> GetInvoker(string serviceName)
        {
            var response = await client.Catalog.Service($"{serviceName}-80");
            var service = response.Response.First();
            var channel = GrpcChannel.ForAddress($"http://{service.ServiceAddress}:{service.ServicePort}");

            // Append the authorization header if present
            var invoker = channel.Intercept((source) =>
            {
                var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                if (token != StringValues.Empty)
                {
                    source.Add("Authorization", token);
                }
                return source;
            });

            return invoker;

        }

        const string IdentityService = "identityservice";
        const string ExpensesService = "expensesservice";

        public async Task<Authentication.AuthenticationClient> GetAuthenticationClientAsync()
        {
            return new Authentication.AuthenticationClient(await GetInvoker(IdentityService));
        }

        public async Task<Account.AccountClient> GetAccountClientAsync()
        {
            return new Account.AccountClient(await GetInvoker(IdentityService));
        }

        public async Task<Domains.DomainsClient> GetDomainsClientAsync()
        {
            return new Domains.DomainsClient(await GetInvoker(IdentityService));
        }
        public async Task<Projects.ProjectsClient> GetProjectsClientAsync()
        {
            return new Projects.ProjectsClient(await GetInvoker(IdentityService));
        }
        public async Task<Users.UsersClient> GetUsersClientAsync()
        {
            return new Users.UsersClient(await GetInvoker(IdentityService));
        }

        public async Task<Balance.BalanceClient> GetBalanceClient()
        {
            return new Balance.BalanceClient(await GetInvoker(ExpensesService));
        }
    }
}
