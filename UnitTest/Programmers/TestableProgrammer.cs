using Domain.Programmers;
using Domain.ProjectManagers;

namespace UnitTest.Programmers
{
    internal class TestableProgrammer : Programmer
    {
        public TestableProgrammer(string name, string phone, string email, ProgrammerRole role, bool isIntern, ProjectManager? manager = null)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            IsIntern = isIntern;
            ProjectManagerId = manager?.Id;
            ProjectManager = manager;
        }
    }
}
