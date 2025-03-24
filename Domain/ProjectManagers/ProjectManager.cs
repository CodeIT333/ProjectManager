using Domain.Commons;
using Domain.Commons.Models;
using Domain.Programmers;
using Domain.Projects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.ProjectManagers
{
    public class ProjectManager : Entity<Guid>
    {
        [Required, MaxLength(200)]
        public string Name { get; protected set; }
        [Required, MaxLength(100)]
        public string Email { get; protected set; }
        [Required, MaxLength(20)]
        public string Phone { get; protected set; }
        public Address Address { get; protected set; }
        public DateOnly DateOfBirth { get; protected set; }
        public List<Project> Projects { get; protected set; } = [];
        public List<Programmer> Employees { get; protected set; } = [];
        [DefaultValue(false)]
        public bool IsArchived { get; protected set; } = false;

        public static ProjectManager Create(
            string name,
            string email,
            string phone,
            Address address,
            DateOnly dateOfBirth,
            List<Programmer>? employees
            )
        {
            return new ProjectManager
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Phone = phone,
                Address = address,
                DateOfBirth = dateOfBirth,
                Employees = employees ?? []
            };
        }

        public void Delete()
        {
            Projects.Clear();
            Employees.Clear();
            IsArchived = true;
        }
    }
}
