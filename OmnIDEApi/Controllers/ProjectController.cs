using Microsoft.AspNetCore.Mvc;
using OmnIDEApi.Models;
using OmnIDEApi.Test;
using OmnIDEApi.Repositories;

namespace OmnIDEApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            
            var projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Name = "OmnIDE",
                    Description = "Next Generation IDE",
                    CreatedDate = DateTime.Now,
                    Language = "TypeScript",
                    Status = "Active"
                }
            };
            var zipTest = new PythonBridgeZipTest();
            zipTest.TestExtractZip();

            var compileTest = new PythonBridgeCompileTest();
            compileTest.TestCompileDirectory();

            var projects = await _projectRepository.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            project.CreatedDate = DateTime.UtcNow;
            var createdProject = await _projectRepository.CreateAsync(project);
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
                return BadRequest();

            await _projectRepository.UpdateAsync(project);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}