using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcademyCloud.Shared
{
    public class TokenClaims
    {
        public bool System { get; set; }

        public string UserId { get; set; }

        public string DomainId { get; set; }

        public string? ProjectId { get; set; }

        public UserRole Role { get; set; }

        public List<Claim> ToClaims()
        {
            return new List<Claim>()
            {
                new Claim(nameof(System), System.ToString()),
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(DomainId), DomainId),
                new Claim(nameof(ProjectId), ProjectId),
            };
        }

        public static TokenClaims FromClaimPrincinpal(ClaimsPrincipal claims)
        {
            return new TokenClaims()
            {
                System = Convert.ToBoolean(claims.FindFirst(nameof(System)).Value),
                UserId = claims.FindFirst(nameof(UserId)).Value,
                DomainId = claims.FindFirst(nameof(DomainId)).Value,
                ProjectId = claims.FindFirst(nameof(ProjectId)).Value,
            };
        }


    }
}
