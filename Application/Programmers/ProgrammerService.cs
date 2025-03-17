using Application.Programmers.DTOs;
using Mapster;

namespace Application.Programmers
{
    public class ProgrammerService
    {
        private readonly IProgrammerRepository _programmerRepo;

        public ProgrammerService(
            IProgrammerRepository programmerRepo
            )
        {
            _programmerRepo = programmerRepo;
        }

        public async Task<List<ProgrammerListDTO>> ListProgrammersAsync()
        {
            var data = await _programmerRepo.ListProgrammersAsync();
            return data.Adapt<List<ProgrammerListDTO>>();
        }
    }
}
