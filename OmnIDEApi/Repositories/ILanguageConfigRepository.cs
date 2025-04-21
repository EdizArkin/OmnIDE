using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public interface ILanguageConfigRepository
    {
        Task<IEnumerable<LanguageConfig>> GetAllAsync();
        Task<LanguageConfig?> GetByIdAsync(int id);
        Task<LanguageConfig?> GetByLanguageNameAsync(string languageName);
        Task<LanguageConfig> AddAsync(LanguageConfig config);
        Task<LanguageConfig?> UpdateAsync(LanguageConfig config);
        Task<bool> DeleteAsync(int id);
    }
}
