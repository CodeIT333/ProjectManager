using Application.Commons.DTOs;
using Application.Programmers.DTOs;
using Application.ProjectManagers.DTOs;
using Application.Projects.DTOs;
using Domain.Projects;
using Mapster;

namespace Application.Projects.ProjectTypeMappings
{
    public class ProjectTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<Project, ProjectListDTO>()
                .MapWith(src => new ProjectListDTO
                {
                    id = src.Id,
                    projectManagerName = src.ProjectManager.Name,
                    customerName = src.Customer.Name,
                    programmerNames = src.ProgrammerProjects.Select(pp => pp.Programmer.Name).ToList(),
                    startDate = src.StartDate,
                    description = src.Description
                });

            // Entity -> DTO
            config.NewConfig<Project, ProjectInProgrammerGetDTO>()
                .MapWith(src => new ProjectInProgrammerGetDTO
                {
                    id = src.Id,
                    projectManagerName = src.ProjectManager.Name,
                    startDate = src.StartDate,
                    description = src.Description
                });

            // Entity -> DTO
            config.NewConfig<Project, ProjectInProjectManagerGetDTO>()
                .MapWith(src => new ProjectInProjectManagerGetDTO
                {
                    projectId = src.Id,
                    projectDescription = src.Description,
                    customerName = src.Customer.Name,
                    customerPhone = src.Customer.Phone,
                    customerEmail = src.Customer.Email
                });

            // Entity -> DTO
            config.NewConfig<Project, ProjectGetDTO>()
                .MapWith(src => new ProjectGetDTO
                {
                    id = src.Id,
                    programmers = src.ProgrammerProjects.Select(pp => pp.Programmer).Adapt<List<ProgrammerInProjectDTO>>(),
                    projectManager = src.ProjectManager.Adapt<ProjectManagerInProgrammerDTO>(),
                    customer = src.Customer.Adapt<CustomerDTO>(),
                    startDate = src.StartDate,
                    description = src.Description
                });
        }
    }
}
