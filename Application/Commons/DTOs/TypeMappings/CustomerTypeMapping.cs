using Domain.Customers;
using Mapster;

namespace Application.Commons.DTOs.TypeMappings
{
    public class CustomerTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity -> DTO
            config.NewConfig<Customer, CustomerDTO>()
                .MapWith(src => new CustomerDTO
                {
                    customerName = src.Name,
                    customerPhone = src.Phone,
                    customerEmail = src.Email,
                });
        }
    }
}
