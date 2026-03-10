using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_WorklogManagement.Domains.Entities
{
    public class TaskItem
    {
        public Guid TaskItemId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AssigneeId { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
