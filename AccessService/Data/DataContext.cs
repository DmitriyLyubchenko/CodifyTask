using AccessService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessService.Data
{
    public class DataContext : DbContext
    {
        public DbSet<ApiKey> ApiKeys { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ApiKey>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<Permission>()
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
