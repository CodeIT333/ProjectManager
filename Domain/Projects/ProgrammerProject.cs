using Domain.Programmers;

namespace Domain.Projects
{
    public class ProgrammerProject
    {
        public Guid ProgrammerId { get; protected set; }
        public Guid ProjectId { get; protected set; }
        public Programmer Programmer { get; protected set; }
        public Project Project { get; protected set; }

        public static ProgrammerProject Create(
            Project project,
            Programmer programmer
            )
        {
            return new ProgrammerProject
            {
                Programmer = programmer,
                ProgrammerId = programmer.Id,
                Project = project,
                ProjectId = project.Id
            };
        }
    }
}
