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
using Task_WorklogManagement.Application.Interfaces.Security;
using Task_WorklogManagement.Domain.Entities;

namespace Task_WorklogManagement.Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }
        public (string token, DateTime expiresAt) CreateAccessAsync(User user)
        {
            var secret = _config["JWT_SECRET"] ?? throw new InvalidOperationException("Missing JWT Secret.");
            var issuer = _config["JWT_ISSUER"] ?? "Task&WorklogManagement";
            var audience = _config["JWT_AUDIENCE"] ?? "Task&WorklogManagementUsers";

            var minutes = _config["JWT_ACCESS_MINUTES"] ?? "30";
            _ = int.TryParse(minutes, out var accessMinutes);
            if (accessMinutes <= 0) accessMinutes = 30;

            var expiresAt = DateTime.UtcNow.AddMinutes(accessMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "MEMBER")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms .HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expiresAt);
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
