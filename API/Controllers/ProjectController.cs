using Application.Projects;
using Application.Projects.DTOs;
using Domain.Commons.Models;
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
        public async Task<ActionResult<List<ProjectListDTO>>> ListProjectsAsync([FromQuery] bool isAvailable)
        {
            var data = await _projectService.ListProjectsAsync(isAvailable);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(ProjectGetDTO))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ProjectGetDTO>> GetProjectManagerAsync(Guid id)
        {
            var data = await _projectService.GetProjectAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [SwaggerResponse(201)]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateProjectAsync(ProjectCreateDTO dto)
        {
            await _projectService.CreateProjectAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(201)]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteProjectAsync(Guid id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}
