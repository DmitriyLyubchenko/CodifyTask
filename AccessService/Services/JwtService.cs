using AccessService.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccessService.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtTokenSettings _jwtTokenSettings;

        public JwtService(IOptions<JwtTokenSettings> jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings.Value;
        }

        public string CreateToken(Guid userId, List<string> permissions)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };

            foreach (var permission in permissions) 
            {
                claims.Add(new Claim("permissions", permission));
            }

            var jwtToken = new JwtSecurityToken(
                _jwtTokenSettings.Issuer,
                audience: null,
                expires: DateTime.UtcNow.AddMinutes(_jwtTokenSettings.Lifetime),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenSettings.Key)), SecurityAlgorithms.HmacSha256)
            );

            var result = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return result;
        }
    }
}
