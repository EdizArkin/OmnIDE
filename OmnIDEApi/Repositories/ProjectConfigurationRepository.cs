using Microsoft.EntityFrameworkCore;
using OmnIDEApi.Data;
using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public class ProjectConfigurationRepository : IProjectConfigurationRepository
    {
        private readonly AppDbContext _context;

        public ProjectConfigurationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectConfiguration>> GetAllAsync()
        {
            return await _context.ProjectConfigurations.ToListAsync();
        }

        public async Task<ProjectConfiguration> GetByIdAsync(int id)
        {
            return await _context.ProjectConfigurations.FindAsync(id);
        }

        public async Task<ProjectConfiguration> GetByPathAsync(string path)
        {
            return await _context.ProjectConfigurations
                .FirstOrDefaultAsync(p => p.ProjectPath == path);
        }

        public async Task<ProjectConfiguration> AddAsync(ProjectConfiguration configuration)
        {
            configuration.CreatedAt = DateTime.Now;
            await _context.ProjectConfigurations.AddAsync(configuration);
            await _context.SaveChangesAsync();
            return configuration;
        }

        public async Task<ProjectConfiguration> UpdateAsync(ProjectConfiguration configuration)
        {
            var existing = await _context.ProjectConfigurations.FindAsync(configuration.Id);
            if (existing == null) return null;

            existing.ProgrammingLanguage = configuration.ProgrammingLanguage;
            existing.ProjectPath = configuration.ProjectPath;
            existing.UpdatedAt = DateTime.Now;

            _context.ProjectConfigurations.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var configuration = await _context.ProjectConfigurations.FindAsync(id);
            if (configuration == null) return false;

            _context.ProjectConfigurations.Remove(configuration);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}