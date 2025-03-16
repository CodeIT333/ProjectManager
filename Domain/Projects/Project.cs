using Domain.Commons.Models;
using Domain.Customers;
using Domain.Programmers;
using Domain.ProjectManagers;

namespace Domain.Projects
{
    public class Project : AggregateRoot<Guid>
    {
        public ProjectManager ProjectManager { get; protected set; }
        public List<Programmer> Employees { get; protected set; } = [];
        public Customer Customer { get; protected set; }
        public DateOnly StartDate { get; protected set; }
        public string Description { get; protected set; }
    }
}
