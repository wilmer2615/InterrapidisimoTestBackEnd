using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataTransferObjects;
using Microsoft.IdentityModel.Tokens;

namespace Interrapidisimo.Security
{
    public class Authentication : IAuthentication
    {
        private IConfiguration _configuration;
        public Authentication(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GenerateToken(StudentDto studentDto)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, studentDto.Id.ToString()),
                new Claim(ClaimTypes.Name, studentDto.Name),
                new Claim(ClaimTypes.Email, studentDto.Email),
                new Claim(ClaimTypes.MobilePhone, studentDto.Phone)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
