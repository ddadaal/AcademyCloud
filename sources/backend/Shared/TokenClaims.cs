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

        public bool Social { get; set; }

        public string UserId { get; set; }

        public string DomainId { get; set; }

        public string? ProjectId { get; set; }
        public string? UserProjectAssignmentId{get;set;}

        public UserRole Role { get; set; }

        public TokenClaims(bool system, bool social, string userId, string domainId, string? projectId, string? userProjectAssignmentId, UserRole role)
        {
            System = system;
            Social = social;
            UserId = userId;
            DomainId = domainId;
            ProjectId = projectId;
            UserProjectAssignmentId = userProjectAssignmentId;
            Role = role;
        }
        public TokenClaims(bool system, bool social, Guid userId, Guid domainId, Guid? projectId, Guid? userProjectAssignmentId, UserRole role)
            : this(system, social, userId.ToString(), domainId.ToString(), projectId?.ToString(), userProjectAssignmentId?.ToString(), role)
        {
        }


        public bool IsSystem => System;

        public bool IsSocial => Social;

        public bool IsDomainScoped => string.IsNullOrEmpty(ProjectId);
        public bool IsDomainAdmin => IsDomainScoped && Role == UserRole.Admin;


        /// <summary>
        /// Is the user authenticated project scoped?
        /// Note that the social is project scoped. Need to check for social first.
        /// </summary>
        public bool IsProjectScoped => !string.IsNullOrEmpty(ProjectId);
        public bool IsProjectAdmin => IsProjectScoped && Role == UserRole.Admin;

        public List<Claim> ToClaims()
        {
            return new List<Claim>()
            {
                new Claim(nameof(System), System.ToString()),
                new Claim(nameof(Social), Social.ToString()),
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(DomainId), DomainId),
                new Claim(nameof(ProjectId), ProjectId ?? ""),
                new Claim(nameof(UserProjectAssignmentId), UserProjectAssignmentId ?? ""),
                new Claim(nameof(Role), Role.ToString()),
            };
        }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(ToClaims()));
        }

        public static TokenClaims FromClaimPrincinpal(ClaimsPrincipal claims)
        {
            return new TokenClaims(
                system: Convert.ToBoolean(claims.GetClaimValue(nameof(System))),
                social: Convert.ToBoolean(claims.GetClaimValue(nameof(Social))),
                userId: claims.GetClaimValue(nameof(UserId)),
                domainId: claims.GetClaimValue(nameof(DomainId)),
                projectId: claims.GetClaimValue(nameof(ProjectId)),
                userProjectAssignmentId: claims.GetClaimValue(nameof(UserProjectAssignmentId)),
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

        public static TokenClaims ToTokenClaims(this ClaimsPrincipal claimsPrincipal)
        {
            return TokenClaims.FromClaimPrincinpal(claimsPrincipal);
        }
    }
}
