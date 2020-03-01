using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcademyCloud.Shared
{
    public class JwtSettings
    {
        public string Issuer { get; private set; }

        public string KeyString { get; private set; }

        public SymmetricSecurityKey Key => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KeyString));

        public JwtSettings(IConfiguration configuration) : this(configuration["Jwt:Issuer"], configuration["Jwt:Key"])
        {
        }

        public JwtSettings(string issuer = "https//academycloud.com", string key = "this key should be as long as possible hahahahahahaaha")
        {
            Issuer = issuer;
            KeyString = key;
        }

        public string GenerateToken(TokenClaims claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Issuer,
                Audience = Issuer,
                Subject = new ClaimsIdentity(claims.ToClaims()),
                SigningCredentials = creds,
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
