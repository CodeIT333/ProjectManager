using Application.Programmers.DTOs;
using Infrastructure.Exceptions;
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
            var programmers = await _programmerRepo.ListProgrammersAsync();
            return programmers.Adapt<List<ProgrammerListDTO>>();
        }

        public async Task<ProgrammerGetDTO> GetProgrammerAsync(Guid id)
        {
            var programmer = await _programmerRepo.GetProgrammerAsync(id);
            if (programmer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            return programmer.Adapt<ProgrammerGetDTO>();
        }
    }
}
