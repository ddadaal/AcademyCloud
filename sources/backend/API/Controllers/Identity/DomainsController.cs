using AcademyCloud.API.Models.Common;
using AcademyCloud.API.Models.Identity.Common;
using AcademyCloud.API.Models.Identity.Domains;
using AcademyCloud.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AcademyCloud.API.Utils.Dummies;

namespace AcademyCloud.API.Controllers.Identity
{
    [Route("/identity/domains")]
    [ApiController]
    [Authorize]
    public class DomainsController : ControllerBase
    {

        private readonly ServiceClientFactory factory;

        public DomainsController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public async Task<ActionResult<GetDomainsResponse>> GetDomains()
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .GetDomainsAsync(new AcademyCloud.Identity.Protos.Domains.GetDomainsRequest
                {

                });

            var subjects = resp.Domains.Select(x => new AcademyCloud.Expenses.Protos.Interop.Subject
            {
                Id = x.Id,
                Type = AcademyCloud.Expenses.Protos.Common.SubjectType.Domain,
            });

            // get pay users
            var payUsers = await (await factory.GetExpensesInteropClientAsync())
                .GetPayUserAsync(new AcademyCloud.Expenses.Protos.Interop.GetPayUserRequest
                {
                    Subjects = { subjects }
                });

            // get pay user's names
            var payUserNames = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new AcademyCloud.Identity.Protos.Interop.GetNamesRequest
                {
                    Subjects =
                    {
                        payUsers.PayUsers.Values.Select(x => new AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.Subject
                        {
                            Id = x,
                            Type = AcademyCloud.Identity.Protos.Interop.GetNamesRequest.Types.SubjectType.User,
                        })
                    }
                });

            // get resources
            var quotas = await (await factory.GetExpensesInteropClientAsync())
                .GetQuotaAsync(new AcademyCloud.Expenses.Protos.Interop.GetQuotaRequest
                {
                    Subjects = { subjects }
                });

            return new GetDomainsResponse
            {
                Domains = resp.Domains.Select(x => new Domain
                {
                    Id = x.Id,
                    Name = x.Name,
                    Active = true,
                    Admins = x.Admins.Select(FromGrpc),
                    PayUser = new User { Id = payUsers.PayUsers[x.Id], Name = payUserNames.IdNameMap[payUsers.PayUsers[x.Id]] },
                    Quota = quotas.Quotas[x.Id],
                })
            };
        }

        [HttpGet("{domainId}/users")]
        public async Task<ActionResult<GetUsersOfDomainResponse>> GetUsersOfDomain([FromRoute] string domainId)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .GetUsersOfDomainAsync(new AcademyCloud.Identity.Protos.Domains.GetUsersOfDomainRequest
                {
                    DomainId = domainId
                });

            var users = await (await factory.GetExpensesInteropClientAsync())
                .GetPayUserAsync(new AcademyCloud.Expenses.Protos.Interop.GetPayUserRequest
                {
                    Subjects =
                    {
                        new AcademyCloud.Expenses.Protos.Interop.Subject{ Id = domainId, Type = AcademyCloud.Expenses.Protos.Common.SubjectType.Domain }
                    }
                });

            return new GetUsersOfDomainResponse
            {
                Admins = resp.Admins.Select(FromGrpc),
                Members = resp.Members.Select(FromGrpc),
                // the pay user should be in admins
                PayUser = FromGrpc(resp.Admins.First(x => x.Id == users.PayUsers[domainId]))
            };
        }

        [HttpPost("{domainId}/users")]
        public async Task<ActionResult> AddUserToDomain([FromRoute] string domainId, [FromBody] AddUserToDomainRequest request)
        {
            // Add in the expenses
            var resp = await (await factory.GetDomainsClientAsync())
                .AddUserToDomainAsync(new AcademyCloud.Identity.Protos.Domains.AddUserToDomainRequest
                {
                    DomainId = domainId,
                    UserId = request.UserId,
                    Role = (AcademyCloud.Identity.Protos.Common.UserRole)request.Role,
                });

            // add user to domain in expenses
            await (await factory.GetExpensesIdentityClient())
                .AddUserToDomainAsync(new AcademyCloud.Expenses.Protos.Identity.AddUserToDomainRequest
                {
                    DomainId = domainId,
                    UserId = request.UserId,
                    UserDomainAssignmentId = resp.UserDomainAssignmentId,
                });

            return NoContent();
        }

        [HttpPatch("{domainId}/users/{userId}")]
        public async Task<ActionResult> ChangeUserRole([FromRoute] string domainId, [FromRoute] string userId, [FromBody] ChangeUserRoleRequest request)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .ChangeUserRoleAsync(new AcademyCloud.Identity.Protos.Domains.ChangeUserRoleRequest
                {
                    DomainId = domainId,
                    UserId = userId,
                    Role = (AcademyCloud.Identity.Protos.Common.UserRole)request.Role,
                });

            return NoContent();

        }

        [HttpPatch("{domainId}/resources")]
        public async Task<ActionResult> SetResources([FromRoute] string domainId, [FromBody] SetResourcesRequest request)
        {
            // set resources in expenses
            await (await factory.GetExpensesIdentityClient())
                .SetDomainQuotaAsync(new AcademyCloud.Expenses.Protos.Identity.SetDomainQuotaRequest
                {
                    DomainId = domainId,
                    Quota = request.Resources,
                });

            return NoContent();
        }

        [HttpPatch("{domainId}/admins")]
        public async Task<ActionResult> SetAdmins([FromRoute] string domainId, [FromBody] SetAdminsRequest request)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .SetAdminsAsync(new AcademyCloud.Identity.Protos.Domains.SetAdminsRequest
                {
                    DomainId = domainId,
                    AdminIds = { request.Ids },
                });

            return NoContent();
        }

        [HttpPatch("{domainId}/payUser")]
        public async Task<ActionResult> SetPayUser([FromRoute] string domainId, [FromBody] SetPayUserRequest request)
        {
            // set the pay user in the expenses

            await (await factory.GetExpensesIdentityClient())
                .SetDomainPayUserAsync(new AcademyCloud.Expenses.Protos.Identity.SetDomainPayUserRequest
                {
                    DomainId = domainId,
                    PayUserId = request.PayUserId,
                });

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateDomain([FromBody] CreateDomainRequest request)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .CreateDomainAsync(new AcademyCloud.Identity.Protos.Domains.CreateDomainRequest
                {
                    Name = request.Name,
                    AdminId = request.PayUserId,
                });

            // sync in expenses
            await (await factory.GetExpensesIdentityClient())
                .AddDomainAsync(new AcademyCloud.Expenses.Protos.Identity.AddDomainRequest
                {
                    Id = resp.DomainId,
                    PayUserId = request.PayUserId,
                    PayUserAssignmentId = resp.AdminDomainAssignmentId,
                });

            return Created(resp.DomainId, resp.DomainId);
        }

        [HttpDelete("{domainId}")]
        public async Task<ActionResult> DeleteDomain([FromRoute] string domainId)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .DeleteDomainAsync(new AcademyCloud.Identity.Protos.Domains.DeleteDomainRequest
                {
                    DomainId = domainId,
                });

            // Delete domain in expenses
            await (await factory.GetExpensesIdentityClient())
               .DeleteDomainAsync(new AcademyCloud.Expenses.Protos.Identity.DeleteDomainRequest
               {
                   Id = domainId,
               });

            return NoContent();
        }

        [HttpDelete("{domainId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromDomain([FromRoute] string domainId, [FromRoute] string userId)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .RemoveUserFromDomainAsync(new AcademyCloud.Identity.Protos.Domains.RemoveUserFromDomainRequest
                {
                    DomainId = domainId,
                    UserId = userId,
                });
            // delete in the expenses
            await (await factory.GetExpensesIdentityClient())
                .RemoveUserFromDomainAsync(new AcademyCloud.Expenses.Protos.Identity.RemoveUserFromDomainRequest
                {
                    DomainId = domainId,
                    UserId = userId,

                });

            return NoContent();

        }

    }
}
