using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Application.Interfaces;
using Task_WorklogManagement.Application.Interfaces.Security;
using Task_WorklogManagement.Application.Models;
using Task_WorklogManagement.Domain.DTOs;

namespace Task_WorklogManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _hash;
        private readonly IJwtTokenService _jwt;
        private readonly IMapper _mapper;
        public AuthService(UserRepository userRepository, RefreshTokenRepository refreshTokenRepository, IPasswordHasher hash, IJwtTokenService jwt, IMapper mapper)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _hash = hash;
            _jwt = jwt;
            _mapper = mapper;
        }
        public Task<AuthenticatedUserModel> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {

        }
        public Task<AuthenticatedUserModel> LoginAsync(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(RefreshTokenDTO refreshTokenDTO)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticatedUserModel> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO)
        {
            throw new NotImplementedException();
        }


    }
}
