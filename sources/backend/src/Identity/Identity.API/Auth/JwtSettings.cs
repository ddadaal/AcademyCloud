using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Auth
{
    public class JwtSettings
    {
        public string Issuer { get; private set; }

        public SymmetricSecurityKey Key { get; private set; }

        public JwtSettings(IConfiguration configuration)
        {
            Issuer = configuration["Jwt:Issuer"];
            var key = configuration["Jwt:Key"];
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}

