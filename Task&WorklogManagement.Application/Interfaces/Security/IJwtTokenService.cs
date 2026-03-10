using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domain.Entities;

namespace Task_WorklogManagement.Application.Interfaces.Security
{
    public interface IJwtTokenService
    {
        (string token, DateTime expiresAt) CreateAccessAsync(User user);
        string GenerateRefreshToken();
    }
}
