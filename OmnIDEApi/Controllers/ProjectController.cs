using Microsoft.AspNetCore.Mvc;
using OmnIDEApi.Models;

namespace OmnIDEApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetProjects()
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

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public ActionResult<Project> GetProject(int id)
        {
            var project = new Project
            {
                Id = id,
                Name = "OmnIDE",
                Description = "Next Generation IDE",
                CreatedDate = DateTime.Now,
                Language = "TypeScript",
                Status = "Active"
            };

            return Ok(project);
        }
    }
}