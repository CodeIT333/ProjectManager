using Application.Commons;
using Application.Customers;
using Application.Programmers;
using Application.Programmers.Specs;
using Application.ProjectManagers;
using Application.ProjectManagers.Specs;
using Application.Projects.DTOs;
using Application.Projects.Specs;
using Domain.Programmers;
using Domain.Projects;
using Infrastructure.Exceptions;
using Mapster;

namespace Application.Projects
{
    public class ProjectService
    {
        private readonly IProgrammerProjectRepository _programmerProjectRepo;
        private readonly IProjectManagerRepository _projectManagerRepos;
        private readonly IProgrammerRepository _programmerRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IUnitOfWork _uow;

        public ProjectService(
            IProgrammerProjectRepository programmerProjectRepo,
            IProjectManagerRepository projectManagerRepo,
            IProgrammerRepository programmerRepo,
            ICustomerRepository customerRepo,
            IProjectRepository projectRepo,
            IUnitOfWork uow
            )
        {
            _programmerProjectRepo = programmerProjectRepo;
            _projectManagerRepos = projectManagerRepo;
            _programmerRepo = programmerRepo;
            _customerRepo = customerRepo;
            _projectRepo = projectRepo;
            _uow = uow;
        }

        public async Task<List<ProjectListDTO>> ListProjectsAsync(bool isAvailable)
        {
            var projects = await _projectRepo.ListProjectsAsync(new ProjectIsAvailableSpec(isAvailable));
            return projects.Adapt<List<ProjectListDTO>>();
        }

        public async Task<ProjectGetDTO> GetProjectAsync(Guid id)
        {
            var project = await _projectRepo.GetProjectAsync(new ProjectIdSpec(id).And(new ProjectIsAvailableSpec(true)));
            if (project is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT);

            if (!project.ProgrammerProjects.Any() || project.ProgrammerProjects.Any(pp => pp.Project is null))
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            return project.Adapt<ProjectGetDTO>();
        }

        public async Task CreateProjectAsync(ProjectCreateUpdateDTO dto)
        {
            var projectManager = await _projectManagerRepos.GetProjectManagerAsync(new ProjectManagerIdSpec(dto.projectManagerId).And(new ProjectManagerIsAvailableSpec(true)));
            if (projectManager is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);

            var customer = await _customerRepo.GetCustomerAsync(dto.customerId);
            if (customer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_CUSTOMER);

            List<Programmer> programmers = new();
            if (dto.programmerIds is not null && dto.programmerIds.Any())
            {
                foreach (var programmerId in dto.programmerIds)
                {
                    var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(programmerId).And(new ProgrammerIsAvailableSpec(true)));
                    if (programmer is null)
                        throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

                    programmers.Add(programmer);
                }
            }

            var project = Project.Create(projectManager, customer, dto.description);
            await _projectRepo.CreateProjectAsync(project);
            await _uow.CommitAsync();

            List<ProgrammerProject> programmerProjects = new();
            foreach (var programmer in programmers)
            {
                var programmerProject = ProgrammerProject.Create(project, programmer);
                await _programmerProjectRepo.CreateProgrammerProjectAsync(programmerProject);
                await _uow.CommitAsync();

                programmer.ProgrammerProjects.Add(programmerProject);
            }

            project.ProgrammerProjects.AddRange(programmerProjects);
            await _uow.CommitAsync();
        }

        public async Task UpdateProjectAsync(Guid id, ProjectCreateUpdateDTO dto)
        {
            var project = await _projectRepo.GetProjectAsync(new ProjectIdSpec(id).And(new ProjectIsAvailableSpec(true)));
            if (project is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT);

            var projectManager = await _projectManagerRepos.GetProjectManagerAsync(
                new ProjectManagerIdSpec(dto.projectManagerId).And(new ProjectManagerIsAvailableSpec(true)));
            if (projectManager is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);

            var customer = await _customerRepo.GetCustomerAsync(dto.customerId);
            if (customer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_CUSTOMER);

            var programmers = new List<Programmer>();
            if (dto.programmerIds is not null && dto.programmerIds.Any())
            {
                foreach (var programmerId in dto.programmerIds)
                {
                    var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(programmerId).And(new ProgrammerIsAvailableSpec(true)));
                    if (programmer is null)
                        throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

                    programmers.Add(programmer);
                }
            }
            
            if (programmers.Any())
            {
                // add
                var programmersToAdd = programmers.Except(project.ProgrammerProjects.Select(pp => pp.Programmer)).ToList();
                var programmerProjectsToAdd = new List<ProgrammerProject>();
                foreach (var programmer in programmersToAdd)
                {
                    var programmerProject = ProgrammerProject.Create(project, programmer);
                    await _programmerProjectRepo.CreateProgrammerProjectAsync(programmerProject);
                    await _uow.CommitAsync();

                    programmerProjectsToAdd.Add(programmerProject);
                }
                
                if (programmerProjectsToAdd.Any()) 
                    project.ProgrammerProjects.AddRange(programmerProjectsToAdd);

                // remove
                var programmersToRemove = project.ProgrammerProjects.Where(pp => !programmers.Contains(pp.Programmer)).ToList();
                var programmerProjectsToRemove = new List<ProgrammerProject>();
                foreach (var programmer in programmersToRemove)
                {
                    project.ProgrammerProjects.Remove(programmer);
                    programmerProjectsToRemove.Add(programmer);
                }

                if (programmerProjectsToRemove.Any())
                    programmerProjectsToRemove.ForEach(pp => _programmerProjectRepo.DeleteProgrammerProject(pp));
            }
            else
            {
                // remove all
                project.ProgrammerProjects.ForEach(pp => _programmerProjectRepo.DeleteProgrammerProject(pp));
                project.ProgrammerProjects.Clear();
            }

            project.Update(
                projectManager, 
                customer, 
                dto.description);

            await _uow.CommitAsync();
        }

        public async Task DeleteProjectAsync(Guid id)
        {
            var project = await _projectRepo.GetProjectAsync(new ProjectIdSpec(id).And(new ProjectIsAvailableSpec(true)));
            if (project is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT);

            if (project.ProgrammerProjects.Any())
            {
                foreach (var programmerProject in project.ProgrammerProjects.ToList())
                {
                    programmerProject.Programmer.ProgrammerProjects.Remove(programmerProject);
                    project.ProgrammerProjects.Remove(programmerProject);
                    _programmerProjectRepo.DeleteProgrammerProject(programmerProject);
                } 
            }

            project.Delete();

            await _uow.CommitAsync();
        }
    }
}
