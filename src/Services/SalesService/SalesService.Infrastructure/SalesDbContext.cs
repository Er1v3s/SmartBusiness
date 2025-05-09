using Microsoft.EntityFrameworkCore;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Category)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasPrecision(5, 2)
                    .HasDefaultValue(0);

                entity.Property(e => e.Tax)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(e => e.ImageFile)
                    .HasDefaultValue(null);

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValue(null);
            });
        }
    }
}
