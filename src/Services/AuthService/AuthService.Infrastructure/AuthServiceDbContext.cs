using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure
{
    public class AuthServiceDbContext : DbContext
    {
        public AuthServiceDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserCompanyRole> UserCompanyRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCompanyRole>(entity =>
            {
                entity.HasKey(ucr => new { ucr.UserId, ucr.CompanyId, ucr.RoleId });
                entity.HasOne(ucr => ucr.User).WithMany(u => u.UserCompanyRoles).HasForeignKey(ucr => ucr.UserId);
                entity.HasOne(ucr => ucr.Company).WithMany(c => c.UserCompanyRoles).HasForeignKey(ucr => ucr.CompanyId);
                entity.HasOne(ucr => ucr.Role).WithMany(r => r.UserCompanyRoles).HasForeignKey(ucr => ucr.RoleId);
            });
                

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.RefreshToken).IsRequired(false);
                entity.Property(e => e.RefreshTokenExpiresAtUtc).IsRequired(false);

                
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });
        }
    }
}
