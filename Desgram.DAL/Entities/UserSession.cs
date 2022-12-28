using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RefreshTokenId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? PushToken { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
