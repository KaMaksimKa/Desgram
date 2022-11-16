﻿namespace Desgram.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Biography { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsPrivate { get; set; }

        public virtual ICollection<Avatar> Avatars { get; set; } = null!;
        public virtual ICollection<UserSubscription> Following { get; set; } = null!;
        public virtual ICollection<UserSubscription> Followers { get; set; } = null!;
        public virtual ICollection<UserSession> Sessions { get; set; } = null!;
        public virtual ICollection<BlockingUser> BlockedUsers { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = null!;

    }
}
