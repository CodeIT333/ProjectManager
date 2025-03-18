using Application.Projects;
using Domain.Programmers;
using Domain.Projects;
using FluentAssertions;
using Moq;
using UnitTest.Commons;
using UnitTest.Customers;
using UnitTest.Programmers;
using UnitTest.ProjectManagers;

namespace UnitTest.Projects
{
    public class ProjectServiceTest
    {
        private readonly Mock<IProjectRepository> _mockRepo;
        private readonly ProjectService _service;

        public ProjectServiceTest()
        {
            TestMapsterConfig.Configure();
            _mockRepo = new Mock<IProjectRepository>();
            _service = new ProjectService(_mockRepo.Object);
        }

        [Fact]
        public async Task ListProjects_ReturnsListOfProjects()
        {
            var projectManager1 = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var projectManager2 = new TestableProjectManager("Bob Williams", "06101234599", "bob@gmail.com");

            var customer1 = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");
            var customer2 = new TestableCustomer("Tech Innovators", "06501234599", "project@techinn.com");

            var programmer1 = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var programmer2 = new TestableProgrammer("Jane Smith", "06207654321", "john@example.com", ProgrammerRole.Backend, true);

            var project1 = new TestableProject(
                projectManager1,
                customer1,
                new List<TestableProgrammer> { programmer1, programmer2 },
                new DateOnly(2024, 3, 18),
                "Project description 1"
            );
            var project2 = new TestableProject(
                projectManager2,
                customer2,
                new List<TestableProgrammer> { programmer1 },
                new DateOnly(2024, 5, 1),
                "Project description 2"
            );

            var programmerProject1 = new TestableProgrammerProject(programmer1, project1);
            var programmerProject2 = new TestableProgrammerProject(programmer2, project1);
            var programmerProject3 = new TestableProgrammerProject(programmer2, project2);

            project1.setProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });
            project2.setProgrammerProjects(new List<ProgrammerProject> { programmerProject3 });

            var mockData = new List<Project> { project1, project2 };

            _mockRepo.Setup(repo => repo.ListProjectsAsync()).ReturnsAsync(mockData);

            var result = await _service.ListProjectsAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].projectManagerName.Should().Be("Alice Johnson");
            result[0].customerName.Should().Be("Acme Corp");
            result[0].startDate.Should().Be(new DateOnly(2024, 3, 18));
            result[0].programmerNames.Should().Contain(new[] { "John Doe", "Jane Smith" });

            result[1].projectManagerName.Should().Be("Bob Williams");
            result[1].customerName.Should().Be("Tech Innovators");
            result[1].startDate.Should().Be(new DateOnly(2024, 5, 1));
            result[1].programmerNames.Should().Contain(new[] { "Jane Smith" });
        }

        [Fact]
        public async Task ListProjects_ReturnsEmptyList()
        {
            var mockData = new List<Project>();

            _mockRepo.Setup(repo => repo.ListProjectsAsync()).ReturnsAsync(mockData);

            var result = await _service.ListProjectsAsync();

            result.Should().BeEmpty();
        }
    }
}
