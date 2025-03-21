using Application.Programmers;
using Domain.Commons;
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

        public async Task<List<Programmer>> ListProgrammersAsync(Specification<Programmer>? spec = null) => 
            await _dbContext.Programmers.AsQueryable().Where(spec?.ToExpressAll() ?? (p => true)).ToListAsync();

        public async Task<Programmer?> GetProgrammerAsync(Specification<Programmer> spec) => await _dbContext.Programmers
            .Include(p => p.ProgrammerProjects).ThenInclude(pp => pp.Project).ThenInclude(p => p.ProjectManager)
            .Include(p => p.ProjectManager)
            .Include(p => p.Address)
            .SingleOrDefaultAsync(spec.ToExpressAll());

        public async Task CreateProgrammerAsync(Programmer programmer) => await _dbContext.Programmers.AddAsync(programmer);
    }
}
