using Application.Programmers.DTOs;
using Application.Programmers;
using Application.ProjectManagers;
using Application.ProjectManagers.DTOs;
using Domain.Commons.Models;
using Infrastructure.Exceptions;
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

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(ProjectManagerGetDTO))]
        [SwaggerResponse(404, ErrorMessages.NOT_FOUND_PROJECT_MANAGER, typeof(ErrorResponse))]
        public async Task<ActionResult<ProjectManagerGetDTO>> GetProjectManagerAsync(Guid id)
        {
            var data = await _projectManagerService.GetProjectManagerAsync(id);
            return Ok(data);
        }
    }
}
