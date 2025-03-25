using Domain.Commons;
using Domain.Projects;

namespace Application.Projects
{
    public interface IProjectRepository
    {
        Task<List<Project>> ListProjectsAsync(Specification<Project>? spec = null);
        Task<Project?> GetProjectAsync(Specification<Project> spec);
        Task CreateProjectAsync(Project project);
    }
}
