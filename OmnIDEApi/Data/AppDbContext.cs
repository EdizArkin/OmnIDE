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
        public DbSet<Student> Students { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<LanguageConfig> LanguageConfigs { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectConfiguration>()
            .HasIndex(p => p.ProjectPath)
            .IsUnique();

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Assignments)
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete if student is deleted
        }
    }
}