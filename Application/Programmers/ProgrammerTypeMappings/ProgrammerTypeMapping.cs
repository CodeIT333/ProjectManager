using Application.Commons.DTOs;
using Application.Programmers.DTOs;
using Application.Projects.DTOs;
using Domain.Programmers;
using Mapster;

namespace Application.Programmers.ProgrammerTypeMappings
{
    public class ProgrammerTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<Programmer, ProgrammerListDTO>()
                .MapWith(src => new ProgrammerListDTO
                {
                    id = src.Id,
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                    role = src.Role,
                    isIntern = src.IsIntern
                });

            // Entity -> DTO
            config.NewConfig<Programmer, ProgrammerGetDTO>()
                .MapWith(src => new ProgrammerGetDTO
                {
                    id = src.Id,
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                    address = src.Address.Adapt<AddressDTO>(),
                    dateOfBirth = src.DateOfBirth,
                    projects = src.ProgrammerProjects.Any() ? 
                        src.ProgrammerProjects
                            .Select(pp => pp.Project)
                            .Adapt<List<ProjectInProgrammerGetDTO>>() :
                        new(),
                    projectManagerId = src.ProjectManagerId,
                    projectManagerName = src.ProjectManager != null ? src.ProjectManager.Name : null,
                    role = src.Role,
                    isIntern = src.IsIntern
                });

            // Entity -> DTO
            config.NewConfig<Programmer, ProgrammerInProjectManagerDTO>()
                .MapWith(src => new ProgrammerInProjectManagerDTO
                {
                    programmerId = src.Id,
                    programmerName = src.Name,
                    programmerPhone= src.Phone,
                    programmerEmail = src.Email,
                    programmerRole = src.Role,
                    programmerIsIntern = src.IsIntern
                });

            // Entity -> DTO
            config.NewConfig<Programmer, ProgrammerInProjectDTO>()
                .MapWith(src => new ProgrammerInProjectDTO
                {
                    programmerId = src.Id,
                    programmerName = src.Name,
                    programmerRole = src.Role,
                    programmerIsIntern = src.IsIntern
                });
        }
    }
}
