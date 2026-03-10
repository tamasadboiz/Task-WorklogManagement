using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Application.Models;
using Task_WorklogManagement.Domain.DTOs;

namespace Task_WorklogManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticatedUserModel> RegisterAsync(UserRegisterDTO userRegisterDTO);
        Task<AuthenticatedUserModel> LoginAsync(UserLoginDTO userLoginDTO);
        Task<AuthenticatedUserModel> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO);
        Task LogoutAsync(RefreshTokenDTO refreshTokenDTO);
    }
}
