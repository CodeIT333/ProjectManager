using Application.ProjectManagers;
using Application.ProjectManagers.DTOs;
using Domain.Commons.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("project-managers")]
    public class ProjectManagerController : ControllerBase
    {
        private readonly ProjectManagerService _projectManagerService;

        public ProjectManagerController(
            ProjectManagerService projectManagerService
            )
        {
            _projectManagerService = projectManagerService;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<ProjectManagerListDTO>))]
        public async Task<ActionResult<List<ProjectManagerListDTO>>> ListProgrammersAsync([FromQuery] bool isAvaiable)
        {
            var data = await _projectManagerService.ListProjectManagersAsync(isAvaiable);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(ProjectManagerGetDTO))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ProjectManagerGetDTO>> GetProjectManagerAsync(Guid id)
        {
            var data = await _projectManagerService.GetProjectManagerAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [SwaggerResponse(201)]
        [SwaggerResponse(400, Type = typeof(ErrorResponse))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateProjectManagerAsync(ProjectManagerCreateDTO dto)
        {
            await _projectManagerService.CreateProjectManagerAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400, Type = typeof(ErrorResponse))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteProjectManagerAsync(Guid id)
        {
            await _projectManagerService.DeleteProjectManagerAsync(id);
            return NoContent();
        }
    }
}
