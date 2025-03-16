using Domain.Commons.Models;
using Domain.Projects;

namespace Domain.Customers
{
    public class Customer : Entity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<Project> Projects { get; set; }
    }
}
