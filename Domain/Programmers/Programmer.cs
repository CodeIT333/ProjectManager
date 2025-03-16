using Domain.Commons;
using Domain.Commons.Models;
using Domain.Projects;

namespace Domain.Programmers
{
    public class Programmer : Entity<Guid>
    {
        public string Name { get; protected set; }
        public Address Address { get; protected set; }
        public DateTime DateOfBirth { get; protected set; }
        public string Phone { get; protected set; }
        public string Email { get; protected set; }
        public List<Project> Projects { get; protected set; } = [];
        public ProgrammerRole Role { get; protected set; }
        public bool IsIntern { get; protected set; }
    }
}
