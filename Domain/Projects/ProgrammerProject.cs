using Domain.Programmers;

namespace Domain.Projects
{
    public class ProgrammerProject
    {
        public Guid ProgrammerId { get; protected set; }
        public Guid ProjectId { get; protected set; }
        public Programmer Programmer { get; protected set; }
        public Project Project { get; protected set; }
    }
}
