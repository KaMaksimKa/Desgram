using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class UserSubscription
    {
        public Guid Id { get; set; }
        public Guid FollowerId { get; set; }
        public Guid ContentMakerId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public bool IsApproved { get; set; }

        public virtual User Follower { get; set; } = null!;
        public virtual User ContentMaker { get; set; } = null!;

    }
}
