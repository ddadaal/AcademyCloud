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
                .GetDomainsAsync(new AcademyCloud.Identity.Services.Domains.GetDomainsRequest
                {

                });

            return new GetDomainsResponse
            {
                Domains = resp.Domains.Select(x => new Domain
                {
                    Id = x.Id,
                    Name = x.Name,
                    Active = true,
                    Admins = x.Admins.Select(TransformUser),
                    PayUser = TransformUser(x.PayUser),
                    Resources = DummyResources,
                })
            };
        }

        [HttpGet("{domainId}/users")]
        public async Task<ActionResult<GetUsersOfDomainResponse>> GetUsersOfDomain([FromRoute] string domainId)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .GetUsersOfDomainAsync(new AcademyCloud.Identity.Services.Domains.GetUsersOfDomainRequest
                {
                    DomainId = domainId
                });

            // TODO get pay user from expenses
            return new GetUsersOfDomainResponse
            {
                Admins = resp.Admins.Select(TransformUser),
                Members = resp.Members.Select(TransformUser),
                PayUser = resp.Admins.Select(TransformUser).First(),
            };
        }

        [HttpPost("{domainId}/users")]
        public async Task<ActionResult> AddUserToDomain([FromRoute] string domainId, [FromBody] AddUserToDomainRequest request)
        {
            // Add in the expenses
            var resp = await (await factory.GetDomainsClientAsync())
                .AddUserToDomainAsync(new AcademyCloud.Identity.Services.Domains.AddUserToDomainRequest
                {
                    DomainId = domainId,
                    UserId = request.UserId,
                    Role = (AcademyCloud.Identity.Services.Common.UserRole)request.Role,
                });

            // TODO add user to domain in expenses
            return NoContent();
        }

        [HttpPatch("{domainId}/users/{userId}")]
        public async Task<ActionResult> ChangeUserRole([FromRoute] string domainId, [FromRoute] string userId, [FromBody] ChangeUserRoleRequest request)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .ChangeUserRoleAsync(new AcademyCloud.Identity.Services.Domains.ChangeUserRoleRequest
                {
                    DomainId = domainId,
                    UserId = userId,
                    Role = (AcademyCloud.Identity.Services.Common.UserRole)request.Role,
                });

            return NoContent();

        }

        [HttpPatch("{domainId}/resources")]
        public async Task<ActionResult> SetResources([FromRoute] string domainId, [FromBody] SetResourcesRequest request)
        {
            // TODO set resources in expenses
            return NoContent();
        }

        [HttpPatch("{domainId}/admins")]
        public async Task<ActionResult> SetAdmins([FromRoute] string domainId, [FromBody] SetAdminsRequest request)
        {
            var resp = await (await factory.GetDomainsClientAsync())
                .SetAdminsAsync(new AcademyCloud.Identity.Services.Domains.SetAdminsRequest
                {
                    DomainId = domainId,
                    AdminIds = { request.Ids },
                });

            return NoContent();
        }

        [HttpPost("{domainId}/payUser")]
        public async Task<ActionResult> SetPayUser([FromRoute] string domainId, [FromBody] SetPayUserRequest request)
        {
            // TODO set the pay user in the expenses
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateDomain([FromBody] CreateDomainRequest request)
        {
            // TODO set the pay user in the expenses
            var resp = await (await factory.GetDomainsClientAsync())
                .CreateDomainAsync(new AcademyCloud.Identity.Services.Domains.CreateDomainRequest
                {
                    Name = request.Name,
                });

            return NoContent();
        }

        [HttpDelete("{domainId}")]
        public async Task<ActionResult> DeleteDomain([FromRoute] string domainId)
        {
            // TODO delete in the expenses
            var resp = await (await factory.GetDomainsClientAsync())
                .DeleteDomainAsync(new AcademyCloud.Identity.Services.Domains.DeleteDomainRequest
                {
                    DomainId = domainId,
                });

            return NoContent();
        }

        [HttpDelete("{domainId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromDomain([FromRoute] string domainId, [FromRoute] string userId)
        {
            // TODO delete in the expenses
            var resp = await (await factory.GetDomainsClientAsync())
                .RemoveUserFromDomainAsync(new AcademyCloud.Identity.Services.Domains.RemoveUserFromDomainRequest
                {
                    DomainId = domainId,
                    UserId = userId,
                });

            return NoContent();

        }

    }
}
