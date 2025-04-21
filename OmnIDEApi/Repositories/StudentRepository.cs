using Microsoft.EntityFrameworkCore;
using OmnIDEApi.Data;
using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.Include(s => s.Assignments).ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Assignments)
                .FirstOrDefaultAsync(s => s.StudentID == id);
        }

        public async Task<Student> AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateAsync(Student student)
        {
            var existing = await _context.Students.FindAsync(student.StudentID);
            if (existing == null) return null;

            existing.Name = student.Name;
            existing.Surname = student.Surname;
            
            _context.Students.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Assignment>> GetStudentAssignmentsAsync(int studentId)
        {
            return await _context.Assignments
                .Where(a => a.StudentID == studentId)
                .ToListAsync();
        }
    }
}
