using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desgram.DAL
{
    public class ApplicationContext:DbContext
    {

        public DbSet<User> Users { get; init; } = null!;
        public DbSet<UserProfile> UserProfiles { get; init; } = null!;
        public DbSet<Comment> Comments { get; init; } = null!;
        public DbSet<ImageUserProfile> ImagesUserProfiles { get; init; } = null!;
        public DbSet<ImagePublication> ImagesPublications { get; init; } = null!;
        public DbSet<LikeComment> LikesComments { get; init; } = null!;
        public DbSet<LikePublication> LikesPublications { get; init; } = null!;
        public DbSet<Publication> Publications { get; init; } = null!;


        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
