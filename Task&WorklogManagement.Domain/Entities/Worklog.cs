using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_WorklogManagement.Domain.Entities
{
    public class Worklog
    {
        public Guid WorklogId { get; set; }
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly WorkDate { get; set; }
        public decimal HoursSpent { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
