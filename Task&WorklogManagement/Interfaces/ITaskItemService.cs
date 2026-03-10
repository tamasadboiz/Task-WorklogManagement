using NpgsqlTypes;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Models;

namespace Task_WorklogManagement.Interfaces
{
    public interface ITaskItemService
    {
        Task<List<TaskItemModel>> GetAllAsync();
        Task<TaskItemModel?> GetByIdAsync(Guid id);
        Task<TaskItemModel> CreateAsync(CreateTaskItemDTO createTaskItemDTO);
        Task<TaskItemModel> UpdateAsync(Guid id, UpdateTaskItemDTO updateTaskItemDTO);
        Task DeleteAsync(Guid id);
    }
}
