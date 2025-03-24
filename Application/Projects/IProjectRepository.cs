using Domain.Commons;
using Domain.Projects;

namespace Application.Projects
{
    public interface IProjectRepository
    {
        Task<List<Project>> ListProjectsAsync();
        Task<Project?> GetProjectAsync(Guid id);
        Task CreateProjectAsync(Project project);
    }
}
