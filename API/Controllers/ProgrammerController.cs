using Application.Programmers;
using Application.Programmers.DTOs;
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
        [SwaggerResponse(200, Type = typeof(List<ProgrammerListDTO>))]
        public async Task<ActionResult<List<ProgrammerListDTO>>> ListProgrammers()
        {
            var data = await _programmerService.ListProgrammers();
            return Ok(data);
        }
        
    }
}
