using Application.Customers;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ProjectManagerContext _dbContext;
        public CustomerRepository(ProjectManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer?> GetCustomerAsync(Guid id) => await _dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
    }
}
