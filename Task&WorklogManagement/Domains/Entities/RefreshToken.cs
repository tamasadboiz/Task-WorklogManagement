using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_WorklogManagement.Domains.Entities
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
