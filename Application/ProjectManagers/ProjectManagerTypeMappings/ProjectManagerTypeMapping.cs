using Application.Commons.DTOs;
using Application.ProjectManagers.DTOs;
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
        }
    }
}
