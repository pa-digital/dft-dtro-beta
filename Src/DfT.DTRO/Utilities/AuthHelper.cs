using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace DfT.DTRO.Utilities
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IConfiguration _configuration;

        public AuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expires = Double.Parse(_configuration["Jwt:ExpireMinutes"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, username)
        }),
                Expires = DateTime.UtcNow.AddMinutes(expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public DateTime GetJwtExpiration(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }

        public string GenerateTwoFactorCode()
        {
            byte[] data = RandomNumberGenerator.GetBytes(4);
            int value = BitConverter.ToInt32(data, 0) & 0x7FFFFFFF;
            int code = value % 1000000;
            return code.ToString("D6");
        }
    }
}