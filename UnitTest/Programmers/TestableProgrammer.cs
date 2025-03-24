using Domain.Commons;
using Domain.Programmers;
using Domain.ProjectManagers;
using Domain.Projects;

namespace UnitTest.Programmers
{
    internal class TestableProgrammer : Programmer
    {
        public TestableProgrammer(string name, string phone, string email, ProgrammerRole role, bool isIntern, ProjectManager? manager = null, bool? isArchived = null)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            IsIntern = isIntern;
            ProjectManagerId = manager?.Id;
            ProjectManager = manager;
            IsArchived = isArchived ?? false;
        }

        public TestableProgrammer(string name, string phone, string email, ProgrammerRole role, bool isIntern, Address address)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            IsIntern = isIntern;
            Address = address;
        }

        public void SetProgrammerProjects(List<ProgrammerProject> programmerProjects)
        {
            ProgrammerProjects = programmerProjects;
        }
    }
}
