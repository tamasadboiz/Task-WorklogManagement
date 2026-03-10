using Task_WorklogManagement.Domains.Enums;

namespace Task_WorklogManagement.Domains.Models
{
    public class TaskItemModel
    {
        public Guid TaskItemId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AssigneeId { get; set; }
        public DateTime Deadline { get; set; }
        public TaskItemPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }
    }
}
