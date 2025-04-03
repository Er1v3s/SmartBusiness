using Microsoft.EntityFrameworkCore;
using SmartBusiness.Domain.Entities;

namespace SmartBusiness.Infrastructure
{
    public class SmartBusinessDbContext : DbContext
    {
        public SmartBusinessDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
            });
        }
    }
}
