using Application.Projects;
using Domain.Commons;
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

        public async Task<List<Project>> ListProjectsAsync(Specification<Project>? spec = null) => 
            await _dbContext.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.Customer)
            .Include(p => p.ProgrammerProjects).ThenInclude(pp => pp.Programmer)
            .Where(spec?.ToExpressAll() ?? (p => !p.IsArchived))
            .ToListAsync();

        public async Task<Project?> GetProjectAsync(Specification<Project> spec) => await _dbContext.Projects
            .Include(p => p.ProgrammerProjects).ThenInclude(pp => pp.Programmer)
            .Include(p => p.ProjectManager)
            .Include(p => p.Customer)
            .SingleOrDefaultAsync(spec.ToExpressAll());

        public async Task CreateProjectAsync(Project project) => await _dbContext.Projects.AddAsync(project);
    }
}
