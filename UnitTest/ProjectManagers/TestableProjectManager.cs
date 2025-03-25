using Domain.Programmers;
using Domain.ProjectManagers;
using Domain.Projects;
using UnitTest.Commons;

namespace UnitTest.ProjectManagers
{
    internal class TestableProjectManager : ProjectManager
    {
        public TestableProjectManager(string name, string phone, string email, bool isArchived = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Phone = phone;
            Email = email;
            IsArchived = isArchived;
        }

        public TestableProjectManager(string name, string phone, string email, DateOnly dateOfBirth, TestableAddress address)
        {
            Id = Guid.NewGuid();
            Name = name;
            Phone = phone;
            Email = email;
            DateOfBirth = dateOfBirth;
            Address = address;
        }

        public void SetProjects(List<Project> projects)
        {
            Projects = projects;
        }

        public void SetEmployees(List<Programmer> employees)
        {
            Employees = employees;
        }
    }
}
