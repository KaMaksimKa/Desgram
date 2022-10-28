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

        public DbSet<Image> Images { get; init; } = null!;

        public DbSet<UserProfile> Profiles { get; init; } = null!;

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
