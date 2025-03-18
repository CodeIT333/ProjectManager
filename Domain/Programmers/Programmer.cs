using Domain.Commons;
using Domain.Commons.Models;
using Domain.ProjectManagers;
using Domain.Projects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Programmers
{
    public class Programmer : Entity<Guid>
    {
        //TODO do I need validation for the others?
        [Required, MaxLength(200)]
        public string Name { get; protected set; }
        [Required, MaxLength(100)]
        public string Email { get; protected set; }
        [Required, MaxLength(20)]
        public string Phone { get; protected set; }
        public Address Address { get; protected set; }
        public DateTime DateOfBirth { get; protected set; }
        public List<ProgrammerProject> ProgrammerProjects { get; protected set; } = [];
        public Guid? ProjectManagerId { get; protected set; }
        public ProjectManager? ProjectManager { get; protected set; }
        public ProgrammerRole Role { get; protected set; }
        public bool IsIntern { get; protected set; }
    }
}
