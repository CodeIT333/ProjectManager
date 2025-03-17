using Application.Programmers;
using Domain.Programmers;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Programmers
{
    public class ProgrammerRepository : IProgrammerRepository
    {
        private readonly ProjectManagerContext _dbContext;
        public ProgrammerRepository(ProjectManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Programmer>> ListProgrammersAsync() => await _dbContext.Programmers.ToListAsync();
    }
}
