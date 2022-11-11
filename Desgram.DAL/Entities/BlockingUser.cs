using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class BlockingUser
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Guid UserId { get; set; }
        public Guid BlockedId { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual User Blocked { get; set; } = null!;
    }
}
