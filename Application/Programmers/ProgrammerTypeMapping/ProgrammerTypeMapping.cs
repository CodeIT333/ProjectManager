using Application.Commons.DTOs;
using Application.Programmers.DTOs;
using Application.Projects.DTOs;
using Domain.Programmers;
using Mapster;

namespace Application.Programmers.ProgrammerTypeMapping
{
    public class ProgrammerTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<Programmer, ProgrammerListDTO>()
                .MapWith(src => new ProgrammerListDTO
                {
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
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                    address = src.Address.Adapt<AddressDTO>(),
                    dateOfBirth = src.DateOfBirth,
                    projects = src.ProgrammerProjects
                        .Where(pp => pp.ProgrammerId == src.Id)
                        .Select(pp => pp.Project)
                        .Adapt<List<ProjectInProgrammerGetDTO>>(),
                    projectManager = src.ProjectManager.Adapt<NameDTO>(),
                    role = src.Role,
                    isIntern = src.IsIntern
                });
        }
    }
}
