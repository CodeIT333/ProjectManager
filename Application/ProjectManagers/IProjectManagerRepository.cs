using Domain.Commons;
using Domain.ProjectManagers;

namespace Application.ProjectManagers
{
    public interface IProjectManagerRepository
    {
        Task<List<ProjectManager>> ListProjectManagersAsync();
        Task<ProjectManager?> GetProjectManagerAsync(Specification<ProjectManager> spec);
        Task CreateProjectManagerAsync(ProjectManager projectManager);
    }
}
