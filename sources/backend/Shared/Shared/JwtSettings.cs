using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AcademyCloud.Shared
{
    public class JwtSettings
    {
        public string Issuer { get; private set; }

        public SymmetricSecurityKey Key { get; private set; }

        public JwtSettings(IConfiguration configuration) : this(configuration["Jwt:Issuer"], configuration["Jwt:Key"])
        {
        }

        public JwtSettings(string issuer = "https//academycloud.com", string key = "this key should be as long as possible hahahahahahaaha")
        {
            Issuer = issuer;
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public string GenerateToken(TokenClaims claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Issuer,
                claims: claims.ToClaims(),
                signingCredentials: creds
                );

            return handler.WriteToken(token);
        }
    }
}
