using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Interfaces.Security;

namespace Task_WorklogManagement.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }
        public (string accessToken, DateTime expiresAt) CreateAccessAsync(User user)
        {
            var secret = _config["Jwt:Key"];
            if(string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("Missing JWT Key.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var issuer = _config["Jwt:Issuer"] ?? "Task&WorklogManagement";
            var audience = _config["Jwt:Audience"] ?? "Task&WorklogManagementUsers";

            var expires = DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:AccessTokenMinutes"] ?? "20"));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
