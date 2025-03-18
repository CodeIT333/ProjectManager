using Application.Projects.DTOs;
using Domain.Projects;
using Mapster;

namespace Application.Projects.ProjectTypeMapping
{
    public class ProjectTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<Project, ProjectListDTO>()
                .MapWith(src => new ProjectListDTO
                {
                    projectManagerName = src.ProjectManager.Name,
                    customerName = src.Customer.Name,
                    programmerNames = src.ProgrammerProjects.Select(pp => pp.Programmer.Name).ToList(),
                    startDate = src.StartDate
                });
        }
    }
}
