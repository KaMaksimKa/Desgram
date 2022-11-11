using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desgram.DAL
{
    public class ApplicationContext:DbContext
    {

        public DbSet<User> Users { get; init; } = null!;
        public DbSet<Comment> Comments { get; init; } = null!;
        public DbSet<Avatar> Avatars { get; init; } = null!;
        public DbSet<AttachPublication> AttachPublications { get; init; } = null!;
        public DbSet<Attach> Attaches { get; init; } = null!;
        public DbSet<LikeComment> LikesComments { get; init; } = null!;
        public DbSet<LikePublication> LikesPublications { get; init; } = null!;
        public DbSet<Like> Likes { get; init; } = null!;
        public DbSet<Publication> Publications { get; init; } = null!;
        public DbSet<UserSession> UserSessions { get; init; } = null!;
        public DbSet<HashTag> HashTags { get; init; } = null!;
        public DbSet<UserSubscription> UserSubscriptions { get; init; } = null!;
        public DbSet<BlockingUser> BlockingUsers { get; init; } = null!;
        public DbSet<UnconfirmedUser> UnconfirmedUsers { get; init; } = null!;
        public DbSet<UnconfirmedEmail> UnconfirmedEmails { get; init; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<HashTag>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<AttachPublication>().ToTable(nameof(AttachPublications));
            modelBuilder.Entity<Avatar>().ToTable(nameof(Avatars));

            modelBuilder.Entity<LikePublication>().ToTable(nameof(LikePublication));
            modelBuilder.Entity<LikeComment>().ToTable(nameof(LikeComment));


            modelBuilder.Entity<User>().HasMany(u => u.Subscribers)
                .WithOne(s => s.Subscription);
            modelBuilder.Entity<User>().HasMany(u => u.Subscriptions)
                .WithOne(s => s.Subscriber);

            modelBuilder.Entity<User>().HasMany(u => u.BlockedUsers)
                .WithOne(b => b.User);
        }
    }
}
