using Application.Projects.DTOs;
using Infrastructure.Exceptions;
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
            var projects = await _projectRepo.ListProjectsAsync();
            return projects.Adapt<List<ProjectListDTO>>();
        }

        public async Task<ProjectGetDTO> GetProjectAsync(Guid id)
        {
            var project = await _projectRepo.GetProjectAsync(id);
            if (project is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT);

            if (!project.ProgrammerProjects.Any() || project.ProgrammerProjects.Any(pp => pp.Project is null))
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            return project.Adapt<ProjectGetDTO>();
        }
    }
}
