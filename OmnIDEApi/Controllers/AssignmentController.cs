using Microsoft.AspNetCore.Mvc;
using OmnIDEApi.Models;
using OmnIDEApi.Repositories;

namespace OmnIDEApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentController(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAll()
        {
            var assignments = await _repository.GetAllAsync();
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetById(string id)
        {
            var assignment = await _repository.GetByIdAsync(id);
            if (assignment == null) return NotFound();
            return Ok(assignment);
        }

        [HttpPost]
        public async Task<ActionResult<Assignment>> Create(Assignment assignment)
        {
            var result = await _repository.AddAsync(assignment);
            return CreatedAtAction(nameof(GetById), new { id = result.AssignmentID }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Assignment>> Update(string id, Assignment assignment)
        {
            if (id != assignment.AssignmentID) return BadRequest();
            var result = await _repository.UpdateAsync(assignment);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
