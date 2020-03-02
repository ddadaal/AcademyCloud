using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services.Domains;
using Grpc.Core;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services.Domains
{
    public class DomainsService : Domains.DomainsBase
    {
        private readonly IdentityDbContext dbContext;

        public DomainsService(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Task<AddUserToDomainResponse> AddUserToDomain(AddUserToDomainRequest request, ServerCallContext context)
        {
            return base.AddUserToDomain(request, context);
        }

        public override Task<ChangeUserRoleResponse> ChangeUserRole(ChangeUserRoleRequest request, ServerCallContext context)
        {
            return base.ChangeUserRole(request, context);
        }

        public override Task<CreateDomainResponse> CreateDomain(CreateDomainRequest request, ServerCallContext context)
        {
            return base.CreateDomain(request, context);
        }

        public override Task<DeleteDomainResponse> DeleteDomain(DeleteDomainRequest request, ServerCallContext context)
        {
            return base.DeleteDomain(request, context);
        }

        public override Task<GetDomainsResponse> GetDomains(GetDomainsRequest request, ServerCallContext context)
        {
            return base.GetDomains(request, context);
        }

        public override Task<GetUsersOfDomainResponse> GetUsersOfDomain(GetUsersOfDomainRequest request, ServerCallContext context)
        {
            return base.GetUsersOfDomain(request, context);
        }

        public override Task<RemoveUserFromDomainResponse> RemoveUserFromDomain(RemoveUserFromDomainRequest request, ServerCallContext context)
        {
            return base.RemoveUserFromDomain(request, context);
        }

        public override Task<SetAdminsResponse> SetAdmins(SetAdminsRequest request, ServerCallContext context)
        {
            return base.SetAdmins(request, context);
        }
    }
}