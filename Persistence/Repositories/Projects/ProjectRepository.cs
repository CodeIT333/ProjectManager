using Application.Projects;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectManagerContext _dbContext;
        public ProjectRepository(ProjectManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> ListProjectsAsync() => 
            await _dbContext.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.Customer)
            .Include(p => p.ProgrammerProjects).ThenInclude(pp => pp.Programmer)
            .ToListAsync();

        public async Task<Project?> GetProjectAsync(Guid id) => await _dbContext.Projects
            .Include(p => p.ProgrammerProjects).ThenInclude(pp => pp.Programmer)
            .Include(p => p.ProjectManager)
            .Include(p => p.Customer)
            .SingleOrDefaultAsync(p => p.Id == id);

        public async Task CreateProjectAsync(Project project) => await _dbContext.Projects.AddAsync(project);
    }
}
