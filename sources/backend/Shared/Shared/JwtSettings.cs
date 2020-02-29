using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
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
    }
}
