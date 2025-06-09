using Microsoft.EntityFrameworkCore;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure
{
    public class SalesServiceDbContext : DbContext
    {
        public SalesServiceDbContext(DbContextOptions<SalesServiceDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                    .IsRequired()
                    .HasMaxLength(21);

                entity.Property(p => p.CompanyId)
                    .IsRequired()
                    .HasMaxLength(17);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Description)
                    .HasMaxLength(500);

                entity.Property(p => p.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasPrecision(7, 2);

                entity.Property(p => p.Tax)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(p => p.CreatedAt)
                    .IsRequired();
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(p => p.CompanyId)
                    .IsRequired()
                    .HasMaxLength(17);

                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Description)
                    .HasMaxLength(500);

                entity.Property(s => s.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.Price)
                    .IsRequired()
                    .HasPrecision(7, 2);

                entity.Property(s => s.Tax)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(s => s.CreatedAt)
                    .IsRequired();

                entity.Property(s => s.Duration)
                    .HasDefaultValue(null);
            });
        }

    }
}
