using Application.Projects;
using Domain.Programmers;
using Domain.Projects;
using FluentAssertions;
using Infrastructure.Exceptions;
using Moq;
using UnitTest.Commons;
using UnitTest.Configurations;
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

        /*--------------------------------------------------------List-------------------------------------------------------*/
        [Fact]
        public async Task ListProjects_ReturnsListOfProjects()
        {
            var projectManager1 = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var projectManager2 = new TestableProjectManager("Bob Williams", "06101234599", "bob@gmail.com");

            var customer1 = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");
            var customer2 = new TestableCustomer("Tech Innovators", "06501234599", "project@techinn.com");

            var programmer1 = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var programmer2 = new TestableProgrammer("Jane Smith", "06207654321", "jane@example.com", ProgrammerRole.Backend, true);

            var project1 = new TestableProject(
                projectManager1,
                customer1,
                new DateOnly(2024, 3, 18),
                "Project description 1"
            );
            var project2 = new TestableProject(
                projectManager2,
                customer2,
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

        /*--------------------------------------------------------Get-------------------------------------------------------*/
        [Fact]
        public async Task GetProjectById_ReturnsProjectWithManagerCustomerAndProgrammers()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var customer = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");

            var programmer1 = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var programmer2 = new TestableProgrammer("Jane Smith", "06207654321", "jane@example.com", ProgrammerRole.Backend, true);

            var project = new TestableProject(
                projectManager,
                customer,
                new DateOnly(2024, 3, 18),
                "Project description"
            );

            var programmerProject1 = new TestableProgrammerProject(programmer1, project);
            var programmerProject2 = new TestableProgrammerProject(programmer2, project);
            project.setProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });

            _mockRepo.Setup(repo => repo.GetProjectAsync(project.Id)).ReturnsAsync(project);

            var result = await _service.GetProjectAsync(project.Id);

            result.Should().NotBeNull();
            result.projectManager.projectManagerName.Should().Be("Alice Johnson");
            result.projectManager.projectManagerEmail.Should().Be("alice@gmail.com");

            result.startDate.Should().Be(new DateOnly(2024, 3, 18));
            result.description.Should().Be("Project description");

            result.customer.customerName.Should().Be("Acme Corp");
            result.customer.customerPhone.Should().Be("06501234566");
            result.customer.customerEmail.Should().Be("project@acme.com");

            result.programmers.Should().HaveCount(2);
            result.programmers[0].programmerName.Should().Be("John Doe");
            result.programmers[0].programmerRole.Should().Be(ProgrammerRole.FullStack);
            result.programmers[0].programmerIsIntern.Should().BeFalse();

            result.programmers[1].programmerName.Should().Be("Jane Smith");
            result.programmers[1].programmerRole.Should().Be(ProgrammerRole.Backend);
            result.programmers[1].programmerIsIntern.Should().BeTrue();
        }

        [Fact]
        public async Task GetProjectById_Returns404ProgrammerNotFoundError()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var customer = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");

            var project = new TestableProject(
                projectManager,
                customer,
                new DateOnly(2024, 3, 18),
                "Project without programmers"
            );

            _mockRepo.Setup(repo => repo.GetProjectAsync(project.Id)).ReturnsAsync(project);

            await FluentActions
                .Invoking(() => _service.GetProjectAsync(project.Id))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROGRAMMER);
        }

        [Fact]
        public async Task GetProjectByNotExistingProjectId_Returns404ProjectError()
        {
            var notExistingId = Guid.NewGuid();
            var mockData = (TestableProject?)null;

            _mockRepo.Setup(repo => repo.GetProjectAsync(notExistingId)).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProjectAsync(notExistingId))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROJECT);
        }
    }
}
