using Application.ProjectManagers.DTOs;
using Domain.ProjectManagers;
using Mapster;

namespace Application.ProjectManagers.ProjectManagerTypeMapping
{
    public class ProjectManagerTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<ProjectManager, ProjectManagerListDTO>()
                .MapWith(src => new ProjectManagerListDTO
                {
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                });
        }
    }
}
