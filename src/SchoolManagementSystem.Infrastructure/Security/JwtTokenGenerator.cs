using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementSystem.Infrastructure.Security
{
    public class JwtSetting
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinute { get; set; } = 60;

    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSetting _settings;
        public JwtTokenGenerator(JwtSetting settings)
        {
            _settings = settings;
        }
        public string GenerateToken(User user, IEnumerable<KeyValuePair<string, string>>? extraClaims = null)
        {
            // Write The Neccesary Information On The Card
            var claimList = new List<Claim>
            {
               new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new (JwtRegisteredClaimNames.Email, user.Email),
               new (ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
               new (ClaimTypes.Role, user.Role)
            };

            if(extraClaims != null)
            {
                claimList.AddRange(extraClaims.Select(kv =>
                new Claim(kv.Key, kv.Value)
                ));
            }

            // Sign On The Card
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // The Whole Card Information
            var jwtToken = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claimList,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinute),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);

        }
    }
}
