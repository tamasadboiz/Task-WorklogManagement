using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_WorklogManagement.Domains.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string RoleName { get; set; } = "MEMBER";
    }
}
