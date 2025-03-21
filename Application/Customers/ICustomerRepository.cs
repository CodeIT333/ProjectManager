using Domain.Customers;

namespace Application.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerAsync(Guid id);
    }
}
