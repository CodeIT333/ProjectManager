using Domain.ProjectManagers;

namespace Application.ProjectManagers
{
    public interface IProjectManagerRepository
    {
        Task<List<ProjectManager>> ListProjectManagersAsync();
        Task<ProjectManager?> GetProjectManagerAsync(Guid id);
    }
}
