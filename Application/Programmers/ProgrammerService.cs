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

        public async Task<List<ProgrammerListDTO>> ListProgrammers()
        {
            var data = await _programmerRepo.ListProgrammers();
            return data.Adapt<List<ProgrammerListDTO>>();
        }
    }
}
