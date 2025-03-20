using Application.Programmers;
using Application.Programmers.DTOs;
using Domain.Commons.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("programmers")]
    public class ProgrammerController : ControllerBase
    {
        private readonly ProgrammerService _programmerService;

        public ProgrammerController(
            ProgrammerService programmerService
            )
        {
            _programmerService = programmerService;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<ProgrammerListDTO>))] // or ProducesResponseType
        public async Task<ActionResult<List<ProgrammerListDTO>>> ListProgrammersAsync()
        {
            var data = await _programmerService.ListProgrammersAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(ProgrammerGetDTO))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ActionResult<ProgrammerGetDTO>> GetProgrammerAsync(Guid id)
        {
            var data = await _programmerService.GetProgrammerAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [SwaggerResponse(201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ActionResult> CreateProgrammerAsync([FromBody] ProgrammerCreateDTO dto)
        {
            await _programmerService.CreateProgrammerAsync(dto);
            return Ok();
        }
    }
}
