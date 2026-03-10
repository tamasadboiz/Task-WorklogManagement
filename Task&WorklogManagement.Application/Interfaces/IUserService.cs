using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domain.Entities;
using Task_WorklogManagement.Domain.Models;

namespace Task_WorklogManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<UserModel> GetByIdAsync(Guid userId);
        Task<UserModel> CreateAsync(string email, string password, int roleId);
        Task<UserModel> UpdateAsync(Guid userId, string? email, string? password, int? roleId);
        Task DeleteAsync(Guid userId);
    }
}
