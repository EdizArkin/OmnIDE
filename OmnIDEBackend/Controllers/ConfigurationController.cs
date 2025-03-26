using Microsoft.AspNetCore.Mvc;
using OmnIDEBackend.Models;

namespace OmnIDEBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        // Örnek olarak bellekte tutulan yapılandırma ayarları
        private static ConfigurationSettings _config = new ConfigurationSettings
        {
            Setting1 = "Default value",
            Setting2 = 10
        };

        // GET: api/configuration
        [HttpGet]
        public IActionResult GetConfiguration()
        {
            return Ok(_config);
        }

        // POST: api/configuration
        [HttpPost]
        public IActionResult UpdateConfiguration([FromBody] ConfigurationSettings config)
        {
            if (config == null)
            {
                return BadRequest();
            }
            _config = config;
            return Ok(_config);
        }
    }
}
