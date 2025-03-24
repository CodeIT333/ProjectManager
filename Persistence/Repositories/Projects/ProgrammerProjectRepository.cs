using Application.Projects;
using Domain.Projects;

namespace Persistence.Repositories.Projects
{
    public class ProgrammerProjectRepository : IProgrammerProjectRepository
    {
        private readonly ProjectManagerContext _dbContext;
        public ProgrammerProjectRepository(ProjectManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateProgrammerProjectAsync(ProgrammerProject programmerProject) => 
            await _dbContext.ProgrammerProjects.AddAsync(programmerProject);

        public void DeleteProgrammerProject(ProgrammerProject programmerProject) => _dbContext.ProgrammerProjects.Remove(programmerProject);
    }
}
