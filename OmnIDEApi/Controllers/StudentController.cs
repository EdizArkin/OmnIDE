using Microsoft.AspNetCore.Mvc;
using OmnIDEApi.Models;
using OmnIDEApi.Repositories;

namespace OmnIDEApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repository;

        public StudentController(IStudentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _repository.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet("{id}/assignments")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetStudentAssignments(int id)
        {
            var assignments = await _repository.GetStudentAssignmentsAsync(id);
            return Ok(assignments);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Create(Student student)
        {
            var result = await _repository.AddAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = result.StudentID }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> Update(int id, Student student)
        {
            if (id != student.StudentID) return BadRequest();
            var result = await _repository.UpdateAsync(student);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
