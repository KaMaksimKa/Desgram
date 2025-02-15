﻿namespace Desgram.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsPrivate { get; set; }

        public virtual ICollection<Avatar> Avatars { get; set; } = null!;
        public virtual ICollection<UserSubscription> Following { get; set; } = null!;
        public virtual ICollection<UserSubscription> Followers { get; set; } = null!;
        public virtual ICollection<UserSession> Sessions { get; set; } = null!;
        public virtual ICollection<BlockingUser> BlockedUsers { get; set; } = null!;
        public virtual ICollection<BlockingUser> UsersBlockedMe { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = null!;
        public virtual ICollection<ApplicationRole> Roles { get; set; } = null!;
        public virtual ICollection<Notification> Notifications { get; set; } = null!;

    }
}
