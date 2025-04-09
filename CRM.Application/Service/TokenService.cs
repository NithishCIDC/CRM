using CRM.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;


namespace CRM.Application.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Guid userId, string email, Guid organizationId, Guid roleId, IEnumerable<string> permission)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var claims = new List<Claim>
                {
                    new("UserId", userId.ToString()),
                    new(ClaimTypes.Email,email),
                    new("OrgId", organizationId.ToString()),
                    new(ClaimTypes.Role, roleId.ToString()),
                };
            foreach (var permissions in permission)
            {
                claims.Add(new Claim("Permission",permissions));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
