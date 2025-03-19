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
                    startDate = src.StartDate
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
        }
    }
}
