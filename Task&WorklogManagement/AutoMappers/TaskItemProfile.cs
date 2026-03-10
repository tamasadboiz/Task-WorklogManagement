using AutoMapper;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Domains.Models;

namespace Task_WorklogManagement.AutoMappers
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<CreateTaskItemDTO, TaskItem>();
            CreateMap<UpdateTaskItemDTO, TaskItem>();
            CreateMap<TaskItem, TaskItemModel>();
        }
    }
}
