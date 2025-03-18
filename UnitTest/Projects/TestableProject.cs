using Domain.Customers;
using Domain.ProjectManagers;
using Domain.Projects;
using UnitTest.Programmers;

namespace UnitTest.Projects
{
    internal class TestableProject : Project
    {
        public TestableProject(ProjectManager manager, Customer customer, List<TestableProgrammer> programmers, DateOnly startDate, string description)
        {
            ProjectManagerId = manager.Id;
            ProjectManager = manager;
            CustomerId = customer.Id;
            Customer = customer;
            StartDate = startDate;
            Description = description;
        }

        public void setProgrammerProjects(List<ProgrammerProject> programmerProjects)
        {
            ProgrammerProjects = programmerProjects;
        }
    }
}
