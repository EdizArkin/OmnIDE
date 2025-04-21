using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student> AddAsync(Student student);
        Task<Student?> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Assignment>> GetStudentAssignmentsAsync(int studentId);
    }
}
