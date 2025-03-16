using Application.Programmers;
using Application.Programmers.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/programmers")]
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
        public async Task<ActionResult<List<ProgrammerListDTO>>> GetProgrammers()
        {
            var data = await _programmerService.ListProgrammers();
            return Ok(data);
        }
        
    }
}
