using Application.Projects.DTOs;
using Mapster;

namespace Application.Projects
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepo;

        public ProjectService(
            IProjectRepository projectRepo
            )
        {
            _projectRepo = projectRepo;
        }

        public async Task<List<ProjectListDTO>> ListProjectsAsync()
        {
            var data = await _projectRepo.ListProjectsAsync();
            return data.Adapt<List<ProjectListDTO>>();
        }
    }
}
