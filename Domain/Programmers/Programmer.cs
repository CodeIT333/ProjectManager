using Domain.Commons;
using Domain.Commons.Models;
using Domain.ProjectManagers;
using Domain.Projects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Programmers
{
    public class Programmer : Entity<Guid>
    {
        [Required, MaxLength(200)]
        public string Name { get; protected set; }
        [Required, MaxLength(100)]
        public string Email { get; protected set; }
        [Required, MaxLength(20)]
        public string Phone { get; protected set; }
        public Address Address { get; protected set; }
        public DateOnly DateOfBirth { get; protected set; }
        public List<ProgrammerProject> ProgrammerProjects { get; protected set; } = [];
        public Guid? ProjectManagerId { get; protected set; }
        public ProjectManager? ProjectManager { get; protected set; }
        public ProgrammerRole Role { get; protected set; }
        public bool IsIntern { get; protected set; }

        public static Programmer Create(
            string name, 
            string email,
            string phone, 
            Address address, 
            DateOnly dateOfBirth, 
            ProgrammerRole role, 
            bool isIntern, 
            ProjectManager? manager)
        {
            return new Programmer
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Phone = phone,
                Address = address,
                DateOfBirth = dateOfBirth,
                Role = role,
                IsIntern = isIntern,
                ProjectManager = manager,
                ProjectManagerId = manager?.Id
            };
        }

        public void Update(
            string name,
            string email,
            string phone,
            DateOnly dateOfBirth,
            ProgrammerRole role,
            bool isIntern,
            ProjectManager? manager)
        {
            UpdateProjectManager(manager);
            if (Name != name) Name = name;
            if (Email != email) Email = email;
            if (Phone != phone) Phone = phone;
            if (DateOfBirth != dateOfBirth) DateOfBirth = dateOfBirth;
            if (Role != role) Role = role;
            if (IsIntern != isIntern) IsIntern = isIntern;
        }

        public void UpdateProjectManager(ProjectManager? manager)
        {
            if (manager is not null && (ProjectManager is null || ProjectManager != manager))
            {
                ProjectManager = manager;
                ProjectManagerId = manager.Id;
            }
        }
    }
}
