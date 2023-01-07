using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desgram.DAL
{
    public class ApplicationContext:DbContext
    {

        public DbSet<User> Users { get; init; } = null!;
        public DbSet<Comment> Comments { get; init; } = null!;
        public DbSet<Avatar> Avatars { get; init; } = null!;
        public DbSet<Attach> Attaches { get; init; } = null!;
        public DbSet<LikeComment> LikesComments { get; init; } = null!;
        public DbSet<LikePost> LikesPosts { get; init; } = null!;
        public DbSet<Like> Likes { get; init; } = null!;
        public DbSet<Post> Posts { get; init; } = null!;
        public DbSet<UserSession> UserSessions { get; init; } = null!;
        public DbSet<HashTag> HashTags { get; init; } = null!;
        public DbSet<UserSubscription> UserSubscriptions { get; init; } = null!;
        public DbSet<BlockingUser> BlockingUsers { get; init; } = null!;
        public DbSet<ApplicationRole> ApplicationRoles { get; init; } = null!;
        public DbSet<PostImageContent> PostImageContents { get; init; } = null!;
        public DbSet<Image> Images { get; init; } = null!;
        public DbSet<ImageContent> ImageContents { get; init; } = null!;
        public DbSet<EmailCode> EmailCodes { get; init; } = null!;
        public DbSet<Notification> Notifications { get; init; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<HashTag>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<Image>().ToTable(nameof(Images));

            modelBuilder.Entity<Avatar>().ToTable(nameof(Avatars));
            modelBuilder.Entity<PostImageContent>().ToTable(nameof(PostImageContents));

            modelBuilder.Entity<LikePost>().ToTable(nameof(LikePost));
            modelBuilder.Entity<LikeComment>().ToTable(nameof(LikeComment));


            modelBuilder.Entity<User>().HasMany(u => u.Followers)
                .WithOne(s => s.ContentMaker);
            modelBuilder.Entity<User>().HasMany(u => u.Following)
                .WithOne(s => s.Follower);

            modelBuilder.Entity<User>().HasMany(u => u.BlockedUsers)
                .WithOne(b => b.User);
            modelBuilder.Entity<User>().HasMany(u => u.UsersBlockedMe)
                .WithOne(b => b.Blocked);

        }
    }
}
