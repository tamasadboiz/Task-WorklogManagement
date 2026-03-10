using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Domains.Models;

namespace Task_WorklogManagement.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllAsync();
        Task<UserModel> GetByIdAsync(Guid userId);
        Task<UserModel> CreateAsync(UserDTO userDTO);
        Task<UserModel> UpdateAsync(Guid userId, UserDTO userDTO);
        Task DeleteAsync(Guid userId);
    }
}
