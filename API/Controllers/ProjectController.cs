using Application.Projects;
using Application.Projects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(
            ProjectService projectService
            )
        {
            _projectService = projectService;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<ProjectListDTO>))]
        public async Task<ActionResult<List<ProjectListDTO>>> ListProjectsAsync()
        {
            var data = await _projectService.ListProjectsAsync();
            return Ok(data);
        }
    }
}
