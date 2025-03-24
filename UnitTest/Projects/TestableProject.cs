using Domain.Customers;
using Domain.ProjectManagers;
using Domain.Projects;

namespace UnitTest.Projects
{
    internal class TestableProject : Project
    {
        public TestableProject(ProjectManager manager, Customer customer, DateOnly startDate, string description)
        {
            Id = Guid.NewGuid();
            ProjectManagerId = manager.Id;
            ProjectManager = manager;
            CustomerId = customer.Id;
            Customer = customer;
            StartDate = startDate;
            Description = description;
        }

        public void SetProgrammerProjects(List<ProgrammerProject> programmerProjects)
        {
            ProgrammerProjects = programmerProjects;
        }
    }
}
