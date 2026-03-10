using Task_WorklogManagement.Domains.Enums;

namespace Task_WorklogManagement.Domains.DTOs
{
    public class CreateTaskItemDTO
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AssigneeId { get; set; }
        public DateTime Deadline { get; set; }
        public TaskItemPriority Priority { get; set; }
    }

    public class UpdateTaskItemDTO
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AssigneeId { get; set; }
        public DateTime Deadline { get; set; }
        public TaskItemPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }
    }
}
