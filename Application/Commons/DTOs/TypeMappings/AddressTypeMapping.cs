using Domain.Commons;
using Mapster;

namespace Application.Commons.DTOs.TypeMappings
{
    public class AddressTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Address, AddressDTO>()
                .MapWith(src => new AddressDTO
                {
                    country = src.Country,
                    zipCode = src.ZipCode,
                    county = src.County,
                    settlement = src.Settlement,
                    street = src.Street,
                    houseNumber = src.HouseNumber,
                    door = src.Door,
                });
        }
    }
}
