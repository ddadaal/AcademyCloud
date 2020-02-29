using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Domains.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Services
{
    public class AccountService : Account.AccountBase
    {

        private readonly IdentityDbContext dbContext;

        public AccountService(IdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        public override Task<ExitDomainResponse> ExitDomain(ExitDomainRequest request, ServerCallContext context)
        {
            return base.ExitDomain(request, context);
        }

        [Authorize]
        public override Task<GetJoinableDomainsResponse> GetJoinableDomains(GetJoinableDomainsRequest request, ServerCallContext context)
        {
            return base.GetJoinableDomains(request, context);
        }

        [Authorize]
        public override Task<GetJoinedDomainsResponse> GetJoinedDomains(GetJoinedDomainsRequest request, ServerCallContext context)
        {
            return base.GetJoinedDomains(request, context);
        }

        [Authorize]
        public override Task<GetProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
        {
            return base.GetProfile(request, context);
        }

        [Authorize]
        public override Task<JoinDomainResponse> JoinDomain(JoinDomainRequest request, ServerCallContext context)
        {
            return base.JoinDomain(request, context);
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var newUser = new User(Guid.NewGuid(), request.Username, request.Password, request.Email, false);

            dbContext.Users.Add(newUser);

            await dbContext.SaveChangesAsync();

            // console.log

            return new RegisterResponse();

        }

        [Authorize]
        public override Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            return base.UpdatePassword(request, context);
        }

        [Authorize]
        public override Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            return base.UpdateProfile(request, context);
        }
    }
}

