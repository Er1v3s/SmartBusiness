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
    }
}
