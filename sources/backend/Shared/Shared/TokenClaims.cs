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

        public TokenClaims(bool system, string userId, string domainId, string? projectId, UserRole role)
        {
            System = system;
            UserId = userId;
            DomainId = domainId;
            ProjectId = projectId;
            Role = role;
        }

        public List<Claim> ToClaims()
        {
            return new List<Claim>()
            {
                new Claim(nameof(System), System.ToString()),
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(DomainId), DomainId),
                new Claim(nameof(ProjectId), ProjectId),
                new Claim(nameof(Role), Role.ToString()),
            };
        }

        public static TokenClaims FromClaimPrincinpal(ClaimsPrincipal claims)
        {
            return new TokenClaims(
                system: Convert.ToBoolean(claims.GetClaimValue(nameof(System))),
                userId: claims.GetClaimValue(nameof(UserId)),
                domainId:claims.GetClaimValue(nameof(DomainId)),
                projectId: claims.GetClaimValue(nameof(ProjectId)),
                role: (UserRole)Enum.Parse(typeof(UserRole), claims.GetClaimValue(nameof(Role)))
            );

        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal claims, string name)
        {
            return claims.FindFirst(name).Value;
        }
    }
}
