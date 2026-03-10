using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_WorklogManagement.Models
{
    public class AuthenticatedUserModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
