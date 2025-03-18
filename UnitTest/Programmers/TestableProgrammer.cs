using Domain.Programmers;

namespace UnitTest.Programmers
{
    internal class TestableProgrammer : Programmer
    {
        public TestableProgrammer(string name, string phone, string email, ProgrammerRole role, bool isIntern)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            IsIntern = isIntern;
        }
    }
}
