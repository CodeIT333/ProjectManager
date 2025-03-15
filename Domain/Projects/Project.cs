using Domain.Commons.Models;
using Domain.Customers;
using Domain.Programmers;
using Domain.ProjectManagers;

namespace Domain.Projects
{
    public class Project : Entity<Guid>
    {
        public ProjectManager ProjectManager { get; set; }
        public List<Programmer> Employees { get; set; } = [];
        public Customer Customer { get; set; }
        public DateOnly StartDate { get; set; }
        public string Description { get; set; }
    }
}
