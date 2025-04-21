using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public interface IProjectConfigurationRepository
    {
        Task<IEnumerable<ProjectConfiguration>> GetAllAsync();
        Task<ProjectConfiguration> GetByIdAsync(int id);
        Task<ProjectConfiguration> AddAsync(ProjectConfiguration configuration);
        Task<ProjectConfiguration> UpdateAsync(ProjectConfiguration configuration);
        Task<bool> DeleteAsync(int id);
        Task<ProjectConfiguration> GetByPathAsync(string path);
    }
}