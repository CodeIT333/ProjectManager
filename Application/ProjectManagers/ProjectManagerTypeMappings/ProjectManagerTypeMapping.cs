using Application.Commons.DTOs;
using Application.Programmers.DTOs;
using Application.ProjectManagers.DTOs;
using Application.Projects.DTOs;
using Domain.ProjectManagers;
using Mapster;

namespace Application.ProjectManagers.ProjectManagerTypeMappings
{
    public class ProjectManagerTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<ProjectManager, ProjectManagerListDTO>()
                .MapWith(src => new ProjectManagerListDTO
                {
                    id = src.Id,
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                });

            // Entity -> DTO
            config.NewConfig<ProjectManager, NameDTO>()
                .MapWith(src => new NameDTO
                {
                    name = src.Name
                });

            // Entity -> DTO
            config.NewConfig<ProjectManager, ProjectManagerGetDTO>()
                .MapWith(src => new ProjectManagerGetDTO
                {
                    id = src.Id,
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                    dateOfBirth = src.DateOfBirth,
                    address = src.Address.Adapt<AddressDTO>(),
                    projects = src.Projects.Adapt<List<ProjectInProjectManagerGetDTO>>(),
                    employees = src.Employees.Adapt<List<ProgrammerInProjectManagerDTO>>(),
                });
        }
    }
}
