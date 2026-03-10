using AutoMapper;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Domains.Enums;
using Task_WorklogManagement.Domains.Models;
using Task_WorklogManagement.Interfaces;
using Task_WorklogManagement.Repositories;

namespace Task_WorklogManagement.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly TaskItemRepository _taskItemRepository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        public TaskItemService(TaskItemRepository taskItemRepository, UserRepository userRepository, IMapper mapper)
        {
            _taskItemRepository = taskItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<TaskItemModel>> GetAllAsync()
        {
            var tasks = await _taskItemRepository.GetAllAsync();
            return _mapper.Map<List<TaskItemModel>>(tasks);
        }

        public async Task<TaskItemModel?> GetByIdAsync(Guid id)
        {
            var task = await _taskItemRepository.GetByIdAsync(id);
            if(task == null)
            {
                throw new KeyNotFoundException("Task Not Found");
            }
            return _mapper.Map<TaskItemModel>(task);
        }

        public async Task<TaskItemModel> CreateAsync(CreateTaskItemDTO createTaskItemDTO)
        {
            if(createTaskItemDTO == null)
            {
                throw new ArgumentNullException(nameof(createTaskItemDTO));
            }

            if (createTaskItemDTO.Deadline <= DateTime.Now)
            {
                throw new ArgumentException("Deadline Invalid.");
            }

            var assignee = await _userRepository.GetByIdAsync(createTaskItemDTO.AssigneeId);
            if(assignee == null)
            {
                throw new KeyNotFoundException("Assignee Not Found.");
            }

            var task = _mapper.Map<TaskItem>(createTaskItemDTO);

            task.TaskItemId = Guid.NewGuid();
            task.Status = (int)TaskItemStatus.Open;
            task.CreatedAt = DateTime.UtcNow;

            await _taskItemRepository.CreateAsync(task);
            return _mapper.Map<TaskItemModel>(task);
        }

        public async Task<TaskItemModel> UpdateAsync(Guid id, UpdateTaskItemDTO updateTaskItemDTO)
        {
            if(updateTaskItemDTO == null)
            {
                throw new ArgumentNullException(nameof(updateTaskItemDTO));
            }

            if(updateTaskItemDTO.Deadline <= DateTime.UtcNow)
            {
                throw new ArgumentException("Deadline Invalid.");
            }

            var task = await _taskItemRepository.GetByIdAsync(id);
            if(task == null)
            {
                throw new KeyNotFoundException("Task Not Found.");
            }

            var assignee = await _userRepository.GetByIdAsync(updateTaskItemDTO.AssigneeId);
            if (assignee == null)
            {
                throw new KeyNotFoundException("Assignee Not Found.");
            }

            _mapper.Map(updateTaskItemDTO, task);

            task.UpdatedAt = DateTime.UtcNow;

            await _taskItemRepository.UpdateAsync(task);
            return _mapper.Map<TaskItemModel>(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _taskItemRepository.GetByIdAsync(id);
            if(task == null)
            {
                throw new KeyNotFoundException("Task Not Found");
            }
            await _taskItemRepository.DeleteAsync(id);
        }
    }
}
