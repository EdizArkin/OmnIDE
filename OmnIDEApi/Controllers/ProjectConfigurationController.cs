using Microsoft.AspNetCore.Mvc;
using OmnIDEApi.Models;
using OmnIDEApi.Repositories;

namespace OmnIDEApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectConfigurationController : ControllerBase
    {
        private readonly IProjectConfigurationRepository _repository;

        public ProjectConfigurationController(IProjectConfigurationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectConfiguration>>> GetAll()
        {
            var configs = await _repository.GetAllAsync();
            return Ok(configs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectConfiguration>> GetById(int id)
        {
            var config = await _repository.GetByIdAsync(id);
            if (config == null) return NotFound();
            return Ok(config);
        }

        [HttpGet("path/{*path}")]
        public async Task<ActionResult<ProjectConfiguration>> GetByPath(string path)
        {
            var config = await _repository.GetByPathAsync(path);
            if (config == null) return NotFound();
            return Ok(config);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectConfiguration>> Create(ProjectConfiguration config)
        {
            var result = await _repository.AddAsync(config);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectConfiguration>> Update(int id, ProjectConfiguration config)
        {
            if (id != config.Id) return BadRequest();
            var result = await _repository.UpdateAsync(config);
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
