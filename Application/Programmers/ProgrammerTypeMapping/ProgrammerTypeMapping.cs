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
            config.NewConfig<ProgrammerListDTO, Programmer>()
                .MapWith(dto => new Programmer
                {
                    Name = dto.name,
                    Phone = dto.phone,
                    Email = dto.email,
                    Role = dto.role,
                    IsIntern = dto.isIntern
                });
        }
    }
}
