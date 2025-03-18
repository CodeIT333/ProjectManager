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
    }
}
