using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domains.Entities;

namespace Task_WorklogManagement.Interfaces.Security
{
    public interface IJwtTokenService
    {
        (string accessToken, DateTime expiresAt) CreateAccessAsync(User user);
        string GenerateRefreshToken();
    }
}
