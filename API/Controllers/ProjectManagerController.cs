using Application.ProjectManagers;
using Application.ProjectManagers.DTOs;
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
        public async Task<ActionResult<List<ProjectManagerListDTO>>> ListProgrammersAsync()
        {
            var data = await _projectManagerService.ListProjectManagersAsync();
            return Ok(data);
        }
    }
}
