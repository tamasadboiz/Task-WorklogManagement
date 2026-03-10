using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Models;

namespace Task_WorklogManagement.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticatedUserModel> RegisterAsync(UserDTO userDTO);
        Task<AuthenticatedUserModel> LoginAsync(UserDTO userDTO);
        Task<AuthenticatedUserModel> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO);
        Task LogoutAsync(RefreshTokenDTO refreshTokenDTO);
    }
}
