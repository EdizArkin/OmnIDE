using Microsoft.EntityFrameworkCore;
using OmnIDEApi.Data;
using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public class LanguageConfigRepository : ILanguageConfigRepository
    {
        private readonly AppDbContext _context;

        public LanguageConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LanguageConfig>> GetAllAsync()
        {
            return await _context.LanguageConfigs.ToListAsync();
        }

        public async Task<LanguageConfig?> GetByIdAsync(int id)
        {
            return await _context.LanguageConfigs.FindAsync(id);
        }

        public async Task<LanguageConfig?> GetByLanguageNameAsync(string languageName)
        {
            return await _context.LanguageConfigs
                .FirstOrDefaultAsync(l => l.LanguageName == languageName);
        }

        public async Task<LanguageConfig> AddAsync(LanguageConfig config)
        {
            await _context.LanguageConfigs.AddAsync(config);
            await _context.SaveChangesAsync();
            return config;
        }

        public async Task<LanguageConfig?> UpdateAsync(LanguageConfig config)
        {
            var existing = await _context.LanguageConfigs.FindAsync(config.ID);
            if (existing == null) return null;

            existing.LanguageName = config.LanguageName;
            existing.CompilerPath = config.CompilerPath;
            existing.IDESettings = config.IDESettings;

            _context.LanguageConfigs.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var config = await _context.LanguageConfigs.FindAsync(id);
            if (config == null) return false;

            _context.LanguageConfigs.Remove(config);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
