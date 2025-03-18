using Domain.Commons.Models;
using Domain.Customers;
using Domain.ProjectManagers;

namespace Domain.Projects
{
    public class Project : AggregateRoot<Guid>
    {
        public Guid ProjectManagerId { get; protected set; }
        public ProjectManager ProjectManager { get; protected set; }
        public List<ProgrammerProject> ProgrammerProjects { get; protected set; } = [];
        public Guid CustomerId { get; protected set; }
        public Customer Customer { get; protected set; }
        public DateOnly StartDate { get; protected set; }
        public string Description { get; protected set; }
    }
}
