using Microsoft.EntityFrameworkCore;
using OmnIDEApi.Data;
using OmnIDEApi.Models;

namespace OmnIDEApi.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assignment>> GetAllAsync()
        {
            return await _context.Assignments.Include(a => a.Student).ToListAsync();
        }

        public async Task<Assignment?> GetByIdAsync(string id)
        {
            return await _context.Assignments
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.AssignmentID == id);
        }

        public async Task<Assignment> AddAsync(Assignment assignment)
        {
            await _context.Assignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<Assignment?> UpdateAsync(Assignment assignment)
        {
            var existing = await _context.Assignments.FindAsync(assignment.AssignmentID);
            if (existing == null) return null;

            existing.Success = assignment.Success;
            existing.StudentID = assignment.StudentID;

            _context.Assignments.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null) return false;

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
