using Domain.Projects;

namespace Application.Projects
{
    public interface IProjectRepository
    {
        Task<List<Project>> ListProjectsAsync();
        Task<Project?> GetProjectAsync(Guid id);
    }
}
