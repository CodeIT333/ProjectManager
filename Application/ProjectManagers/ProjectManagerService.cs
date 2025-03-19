using Application.ProjectManagers.DTOs;
using Infrastructure.Exceptions;
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
            var projectManagers = await _projectManagerRepo.ListProjectManagersAsync();
            return projectManagers.Adapt<List<ProjectManagerListDTO>>();
        }

        public async Task<ProjectManagerGetDTO> GetProjectManagerAsync(Guid id)
        {
            var projectManager = await _projectManagerRepo.GetProjectManagerAsync(id);
            if (projectManager is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            
            return projectManager.Adapt<ProjectManagerGetDTO>();
        }
    }
}
