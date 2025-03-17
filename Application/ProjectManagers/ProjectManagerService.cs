using Application.ProjectManagers.DTOs;
using Mapster;

namespace Application.ProjectManagers
{
    public class ProjectManagerService
    {
        private readonly IProjectManagerRepository _projectManagerRepo;

        public ProjectManagerService(
            IProjectManagerRepository projectManagerRepo
            )
        {
            _projectManagerRepo = projectManagerRepo;
        }

        public async Task<List<ProjectManagerListDTO>> ListProjectManagersAsync()
        {
            var data = await _projectManagerRepo.ListProjectManagersAsync();
            return data.Adapt<List<ProjectManagerListDTO>>();
        }
    }
}
