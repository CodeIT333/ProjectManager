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
    }
}
