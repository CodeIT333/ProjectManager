using Application.ProjectManagers;
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

        public async Task<List<ProjectManager>> ListProjectManagersAsync() => await _dbContext.ProjectManagers.ToListAsync();

        public async Task<ProjectManager?> GetProjectManagerAsync(Guid id) => await _dbContext.ProjectManagers
            .Include(pm => pm.Projects).ThenInclude(p => p.Customer)
            .Include(pm => pm.Employees)
            .Include(pm => pm.Address)
            .SingleOrDefaultAsync(p => p.Id == id);
    }
}
