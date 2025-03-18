using Domain.Programmers;
using Domain.Projects;

namespace UnitTest.Projects
{
    internal class TestableProgrammerProject : ProgrammerProject
    {
        public TestableProgrammerProject(Programmer programmer, Project project)
        {
            Project = project;
            ProjectId = project.Id;
            Programmer = programmer;
            ProgrammerId = programmer.Id;
        }
    }
}
