using Microsoft.AspNetCore.Mvc;
using OmnIDEApi.Models;
using OmnIDEApi.Repositories;

namespace OmnIDEApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageConfigController : ControllerBase
    {
        private readonly ILanguageConfigRepository _repository;

        public LanguageConfigController(ILanguageConfigRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LanguageConfig>>> GetAll()
        {
            var configs = await _repository.GetAllAsync();
            return Ok(configs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageConfig>> GetById(int id)
        {
            var config = await _repository.GetByIdAsync(id);
            if (config == null) return NotFound();
            return Ok(config);
        }

        [HttpGet("language/{name}")]
        public async Task<ActionResult<LanguageConfig>> GetByLanguage(string name)
        {
            var config = await _repository.GetByLanguageNameAsync(name);
            if (config == null) return NotFound();
            return Ok(config);
        }

        [HttpPost]
        public async Task<ActionResult<LanguageConfig>> Create(LanguageConfig config)
        {
            var result = await _repository.AddAsync(config);
            return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LanguageConfig>> Update(int id, LanguageConfig config)
        {
            if (id != config.ID) return BadRequest();
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
