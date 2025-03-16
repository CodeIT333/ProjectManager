using Domain.Commons;
using Domain.Commons.Models;
using Domain.Programmers;
using Domain.Projects;

namespace Domain.ProjectManagers
{
    public class ProjectManager : Entity<Guid>
    {
        public Address Address { get; protected set; }
        public DateTime DateOfBirth { get; protected set; }
        public string Phone { get; protected set; }
        public string Email { get; protected set; }
        public List<Project> Projects { get; protected set; } = [];
        public List<Programmer> Employees { get; protected set; } = [];
    }
}
