using Microsoft.EntityFrameworkCore;
using OmnIDEApi.Models;

namespace OmnIDEApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProjectConfiguration> ProjectConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ProjectConfiguration>()
                .HasIndex(p => p.ProjectPath)
                .IsUnique();
        }
    }
}