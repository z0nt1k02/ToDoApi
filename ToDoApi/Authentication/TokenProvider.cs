using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDoApi.Contracts;
using ToDoApi.Models;

namespace ToDoApi.Authentication;

public class TokenProvider : ITokenProvider
{
        private readonly IConfiguration _configuration;

        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GenerateToken(UserModel user)
        {
            var jwtOptions = _configuration.GetSection("JwtOptions");
            string secretKey = jwtOptions.GetSection("Key").Value!;
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                ]),
                Issuer = jwtOptions.GetSection("Issuer").Value,
                Audience = jwtOptions.GetSection("Audience").Value,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtOptions["TokenValidityMins"])),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
}