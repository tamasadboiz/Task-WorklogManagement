using AutoMapper;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Interfaces;
using Task_WorklogManagement.Interfaces.Security;
using Task_WorklogManagement.Models;
using Task_WorklogManagement.Repositories;

namespace Task_WorklogManagement.Services
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
        public async Task<AuthenticatedUserModel> RegisterAsync(UserDTO userDTO)
        {
            if(string.IsNullOrWhiteSpace(userDTO.Email))
            {
                throw new InvalidOperationException("Email không hợp lệ.");
            }

            if(string.IsNullOrWhiteSpace(userDTO.Password) || userDTO.Password.Length < 8)
            {
                throw new InvalidOperationException("Mật khẩu tối thiểu 8 ký tự");
            }    

            var exists = await _userRepository.ExistByEmailAsync(userDTO.Email);
            if (exists)
            {
                throw new InvalidOperationException("Email đã tồn tại.");
            }    

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = userDTO.Email.Trim().ToLower(),
                PasswordHash = _hash.Hash(userDTO.Password),
                RoleId = 3,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var created = await _userRepository.GetByEmailAsync(user.Email);
            if (created == null)
            {
                throw new InvalidOperationException("Tạo user thất bại.");
            } 

            var (accessToken, _) = _jwt.CreateAccessAsync(created);
            var refreshToken = _jwt.GenerateRefreshToken();

            await _refreshTokenRepository.CreateAsync(new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = created.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            });

            var result = _mapper.Map<AuthenticatedUserModel>(created);
            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;
            return result;
        }
        public async Task<AuthenticatedUserModel> LoginAsync(UserDTO userDTO)
        {
            if(string.IsNullOrWhiteSpace(userDTO.Email) || string.IsNullOrWhiteSpace(userDTO.Password))
            {
                throw new InvalidOperationException("Email hoặc mật khẩu không đúng.");
            }    

            var user = await _userRepository.GetByEmailAsync(userDTO.Email.Trim().ToLower());
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new InvalidOperationException("Email hoặc mật khẩu không đúng.");
            }    

            var valid = _hash.Verify(userDTO.Password, user.PasswordHash);

            if (!valid)
            {
                throw new InvalidOperationException("Email hoặc mật khẩu không đúng.");
            }

            var (accessToken, _) = _jwt.CreateAccessAsync(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            await _refreshTokenRepository.CreateAsync(new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            });

            var result = _mapper.Map<AuthenticatedUserModel>(user);
            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;
            return result;
        }

        public async Task<AuthenticatedUserModel> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO)
        {
            if(string.IsNullOrWhiteSpace(refreshTokenDTO.RefreshToken))
            {
                throw new InvalidOperationException("Refresh Token không hợp lệ.");
            }    

            var token = await _refreshTokenRepository.GetByTokenAsync(refreshTokenDTO.RefreshToken);
            if (token == null)
            {
                throw new InvalidOperationException("Refresh token không tồn tại.");
            }    

            if (token.RevokedAt != null)
            {
                throw new InvalidOperationException("Refresh token đã bị thu hồi.");
            }   
            
            if (DateTime.UtcNow >= token.ExpiresAt)
            {
                throw new InvalidOperationException("Refresh token đã hết hạn.");
            }

            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User không tồn tại.");
            }

            await _refreshTokenRepository.RevokeAsync(refreshTokenDTO.RefreshToken);

            var newRefreshToken = _jwt.GenerateRefreshToken();

            await _refreshTokenRepository.CreateAsync(new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            });

            var (newAccessToken, _) = _jwt.CreateAccessAsync(user);

            var result = _mapper.Map<AuthenticatedUserModel>(user);
            result.AccessToken = newAccessToken;
            result.RefreshToken = newRefreshToken;
            return result;
        }

        public async Task LogoutAsync(RefreshTokenDTO refreshTokenDTO)
        {
            if(!string.IsNullOrWhiteSpace(refreshTokenDTO.RefreshToken))
            {
                await _refreshTokenRepository.RevokeAsync(refreshTokenDTO.RefreshToken);
            }
        }
    }
}
