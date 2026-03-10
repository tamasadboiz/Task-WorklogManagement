using AutoMapper;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Domains.Models;
using Task_WorklogManagement.Interfaces;
using Task_WorklogManagement.Interfaces.Security;
using Task_WorklogManagement.Repositories;

namespace Task_WorklogManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasher _hash;
        private readonly IMapper _mapper;
        public UserService(UserRepository userRepository, IPasswordHasher hash, IMapper mapper)
        {
            _userRepository = userRepository;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            var list = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserModel>>(list);
        }

        public async Task<UserModel> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null) throw new KeyNotFoundException("User Not Found.");
            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateAsync(UserDTO userDTO)
        {
            var exists = await _userRepository.ExistByEmailAsync(userDTO.Email);
            if (exists) throw new InvalidOperationException("Email Already Exists.");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = userDTO.Email.Trim().ToLower(),
                PasswordHash = _hash.Hash(userDTO.Password),
                RoleId = 3,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var created = await _userRepository.GetByIdAsync(user.UserId);
            if (created is null) throw new InvalidOperationException("Create User Failed.");

            return _mapper.Map<UserModel>(created);
        }

        public async Task<UserModel> UpdateAsync(Guid userId, UserDTO userDTO)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null) throw new KeyNotFoundException("User Not Found.");

            user.Email = userDTO.Email.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(userDTO.Password))
            {
                user.PasswordHash = _hash.Hash(userDTO.Password);
            }

            await _userRepository.UpdateAsync(user);

            var updated = await _userRepository.GetByIdAsync(userId);
            if (updated is null)
            {
                throw new InvalidOperationException("Update Failed.");
            }

            return _mapper.Map<UserModel>(updated);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _userRepository.DeleteAsync(userId);
        }
    }
}
