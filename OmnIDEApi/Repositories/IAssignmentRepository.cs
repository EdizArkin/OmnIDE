using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<Assignment>> GetAllAsync();
        Task<Assignment?> GetByIdAsync(string id);
        Task<Assignment> AddAsync(Assignment assignment);
        Task<Assignment?> UpdateAsync(Assignment assignment);
        Task<bool> DeleteAsync(string id);
    }
}
