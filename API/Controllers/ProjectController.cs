using Application.Projects;
using Application.Projects.DTOs;
using Domain.Commons.Models;
using Infrastructure.Exceptions;
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

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(ProjectGetDTO))]
        [SwaggerResponse(404, ErrorMessages.NOT_FOUND_PROJECT, typeof(ErrorResponse))]
        public async Task<ActionResult<ProjectGetDTO>> GetProjectManagerAsync(Guid id)
        {
            var data = await _projectService.GetProjectAsync(id);
            return Ok(data);
        }
    }
}
