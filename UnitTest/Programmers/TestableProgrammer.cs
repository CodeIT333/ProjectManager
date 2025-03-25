using Domain.Commons;
using Domain.Programmers;
using Domain.ProjectManagers;
using Domain.Projects;

namespace UnitTest.Programmers
{
    internal class TestableProgrammer : Programmer
    {
        public TestableProgrammer(string name, string phone, string email, ProgrammerRole role, bool isIntern, ProjectManager? manager = null, bool isArchived = false)
        {

            Id = Guid.NewGuid();
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            IsIntern = isIntern;
            ProjectManagerId = manager?.Id;
            ProjectManager = manager;
            IsArchived = isArchived;
        }

        public TestableProgrammer(string name, string phone, string email, ProgrammerRole role, bool isIntern, Address address, ProjectManager? manager = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            IsIntern = isIntern;
            Address = address;
            ProjectManagerId = manager?.Id;
            ProjectManager = manager;
        }

        public void SetProgrammerProjects(List<ProgrammerProject> programmerProjects)
        {
            ProgrammerProjects = programmerProjects;
        }
    }
}
