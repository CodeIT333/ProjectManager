using Domain.Commons;
using Domain.Commons.Models;
using Domain.Projects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Programmers
{
    public class Programmer : Entity<Guid>
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(20)]
        public string Phone { get; set; }
        public Address Address { get; protected set; }
        public DateTime DateOfBirth { get; protected set; }
        public List<ProgrammerProject> ProgrammerProjects { get; protected set; } = [];
        public Guid? ProjectManagerId { get; protected set; }
        public ProgrammerRole Role { get; protected set; }
        public bool IsIntern { get; protected set; }
    }
}
