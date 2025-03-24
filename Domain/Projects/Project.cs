using Domain.Commons.Models;
using Domain.Customers;
using Domain.ProjectManagers;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(10000)]
        public string Description { get; protected set; }

        public static Project Create(
            ProjectManager projectManager,
            Customer customer,
            string description
            )
        {
            return new Project
            {
                Id = Guid.NewGuid(),
                ProjectManager = projectManager,
                ProjectManagerId = projectManager.Id,
                Customer = customer,
                CustomerId = customer.Id,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Description = description
            };
        }

        public void Update(
            ProjectManager projectManager,
            List<ProgrammerProject>? programmerProjects,
            Customer customer,
            string description
            )
        {
            if (projectManager.Id != ProjectManagerId)
            {
                ProjectManager = projectManager;
                ProjectManagerId = projectManager.Id;
            }
            if (programmerProjects is not null && programmerProjects.Any()) ProgrammerProjects = programmerProjects;
            if (customer.Id != CustomerId)
            {
                Customer = customer;
                CustomerId = customer.Id;
            }
            if (description != Description) Description = description;   
        }
    }
}
