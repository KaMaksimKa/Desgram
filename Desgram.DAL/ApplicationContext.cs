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
        public ApplicationContext() : base() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options) 
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql( b => b.MigrationsAssembly("Desgram.DAL"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
