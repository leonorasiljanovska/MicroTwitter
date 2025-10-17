using Microsoft.EntityFrameworkCore;
using MicroTwitter.Api.Domain.Models;

namespace MicroTwitter.Api.Infrastructure.Data
{
    public class AppDbContext : DbContext 
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
