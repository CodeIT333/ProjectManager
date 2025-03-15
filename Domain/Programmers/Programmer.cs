using Domain.Commons;
using Domain.Commons.Models;
using Domain.Projects;

namespace Domain.Programmers
{
    public class Programmer : Entity<Guid>
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<Project> Projects { get; set; } = [];
        public ProgrammerRole Role { get; set; }
        public bool IsIntern { get; set; }
    }
}
