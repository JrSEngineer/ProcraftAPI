using Microsoft.IdentityModel.Tokens;
using ProcraftAPI.Interfaces;
using ProcraftAPI.Security.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProcraftAPI.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(ProcraftAuthentication authentication)
        {
            var secureKey = Environment.GetEnvironmentVariable("SECURE_KEY");

            var secretServerKey = Encoding.UTF8.GetBytes(secureKey!);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.PrimarySid, authentication.UserId.ToString()!),
                    new Claim(ClaimTypes.Email, authentication.Email),
                    new Claim(ClaimTypes.Role, authentication.Role.ToString()!),
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretServerKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var currentToken = tokenHandler.ReadToken(token);

            bool validToken = DateTime.UtcNow.CompareTo(currentToken.ValidTo) <= 0;

            return validToken;
        }
    }
}
