using Application.ProjectManagers;
using Domain.Commons;
using Domain.ProjectManagers;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.ProjectManagers
{
    public class ProjectManagerRepository : IProjectManagerRepository
    {
        private readonly ProjectManagerContext _dbContext;
        public ProjectManagerRepository(ProjectManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProjectManager>> ListProjectManagersAsync(Specification<ProjectManager>? spec = null) => 
            await _dbContext.ProjectManagers.Where(spec?.ToExpressAll() ?? (pm => !pm.IsArchived)).ToListAsync();

        public async Task<ProjectManager?> GetProjectManagerAsync(Specification<ProjectManager> spec) => await _dbContext.ProjectManagers
            .Include(pm => pm.Projects).ThenInclude(p => p.Customer)
            .Include(pm => pm.Employees)
            .Include(pm => pm.Address)
            .SingleOrDefaultAsync(spec.ToExpressAll());

        public async Task CreateProjectManagerAsync(ProjectManager projectManager) => await _dbContext.ProjectManagers.AddAsync(projectManager);
    }
}
