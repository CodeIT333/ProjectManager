using Application.Programmers.DTOs;
using Domain.Programmers;
using Mapster;

namespace Application.Programmers.ProgrammerTypeMapping
{
    public class ProgrammerTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // DTO -> Entity
            config.NewConfig<Programmer, ProgrammerListDTO>()
                .MapWith(src => new ProgrammerListDTO
                {
                    name = src.Name,
                    phone = src.Phone,
                    email = src.Email,
                    role = src.Role,
                    isIntern = src.IsIntern
                });
        }
    }
}
