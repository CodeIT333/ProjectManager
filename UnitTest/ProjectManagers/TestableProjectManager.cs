using Domain.ProjectManagers;

namespace UnitTest.ProjectManagers
{
    internal class TestableProjectManager : ProjectManager
    {
        public TestableProjectManager(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}
