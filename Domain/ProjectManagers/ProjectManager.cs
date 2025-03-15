using Domain.Commons;
using Domain.Commons.Models;
using Domain.Programmers;
using Domain.Projects;

namespace Domain.ProjectManagers
{
    public class ProjectManager : AggregateRoot<Guid>
    {
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<Project> Projects { get; set; } = [];
        public List<Programmer> Employees { get; set; } = [];

        
    }
}
