using Application.ProjectManagers;
using Domain.Programmers;
using Domain.ProjectManagers;
using Domain.Projects;
using FluentAssertions;
using Infrastructure.Exceptions;
using Moq;
using UnitTest.Commons;
using UnitTest.Configurations;
using UnitTest.Programmers;
using UnitTest.Projects;

namespace UnitTest.ProjectManagers
{
    public class ProjectManagerServiceTest
    {
        private readonly Mock<IProjectManagerRepository> _mockRepo;
        private readonly ProjectManagerService _service;

        public ProjectManagerServiceTest()
        {
            TestMapsterConfig.Configure();
            _mockRepo = new Mock<IProjectManagerRepository>();
            _service = new ProjectManagerService(_mockRepo.Object);
        }

        /*--------------------------------------------------------List-------------------------------------------------------*/
        [Fact]
        public async Task ListProjectManagers_ReturnsListOfProjectManagers()
        {
            var mockData = new List<ProjectManager>
            {
                new TestableProjectManager("Manager X", "987-654-3210", "managerX@example.com"),
                new TestableProjectManager("Manager Y", "876-543-2109", "managerY@example.com")
            };

            _mockRepo.Setup(repo => repo.ListProjectManagersAsync()).ReturnsAsync(mockData);

            var result = await _service.ListProjectManagersAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].name.Should().Be("Manager X");
            result[0].phone.Should().Be("987-654-3210");
            result[0].email.Should().Be("managerX@example.com");
            result[1].name.Should().Be("Manager Y");
            result[1].phone.Should().Be("876-543-2109");
            result[1].email.Should().Be("managerY@example.com");
        }

        [Fact]
        public async Task ListProjectManagers_ReturnsEmptyList()
        {
            var mockData = new List<ProjectManager>();

            _mockRepo.Setup(repo => repo.ListProjectManagersAsync()).ReturnsAsync(mockData);

            var result = await _service.ListProjectManagersAsync();

            result.Should().BeEmpty();
        }

        /*--------------------------------------------------------Get-------------------------------------------------------*/
        [Fact]
        public async Task GetProjectManagerById_ReturnsProjectManagerWithAddressAndProjectsAndWithoutEmployees()
        {
            var projectManagerAddress = new TestableAddress("Hungary", "6722", "Csongrád", "Szeged", "Kossuth Lajos sugárút", "15.", 1);
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com", new DateTime(1990, 1, 1), projectManagerAddress);

            var customer1 = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");
            var customer2 = new TestableCustomer("Tech Innovators", "06501234599", "project@techinn.com");

            var programmer1 = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var programmer2 = new TestableProgrammer("Jane Smith", "06207654321", "jane@example.com", ProgrammerRole.Backend, true);

            var project1 = new TestableProject(
                projectManager,
                customer1,
                new DateOnly(2024, 3, 18),
                "Project description 1"
            );
            var project2 = new TestableProject(
                projectManager,
                customer2,
                new DateOnly(2024, 5, 1), 
                "Project description 2"
            );

            var programmerProject1 = new TestableProgrammerProject(programmer1, project1);
            var programmerProject2 = new TestableProgrammerProject(programmer2, project1);
            var programmerProject3 = new TestableProgrammerProject(programmer2, project2);

            project1.setProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });
            project2.setProgrammerProjects(new List<ProgrammerProject> { programmerProject3 });

            projectManager.SetProjects(new List<Project> { project1, project2 });

            var mockData = projectManager;

            _mockRepo.Setup(repo => repo.GetProjectManagerAsync(projectManager.Id)).ReturnsAsync(mockData);

            var result = await _service.GetProjectManagerAsync(projectManager.Id);
            result.Should().NotBeNull();

            result.name.Should().Be("Alice Johnson");
            result.phone.Should().Be("06101234567");
            result.email.Should().Be("alice@gmail.com");
            result.dateOfBirth.Should().Be(new DateTime(1990, 1, 1));
            result.projects.Should().HaveCount(2);
            result.projects[0].customerName.Should().Be("Acme Corp");
            result.projects[0].customerPhone.Should().Be("06501234566");
            result.projects[0].customerEmail.Should().Be("project@acme.com");
            result.projects[0].projectDescription.Should().Be("Project description 1");
            result.employees.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProjectManagerById_ReturnsProjectManagerWithoutProjectsAndWithEmployees()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");

            var programmer1 = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, projectManager);
            var programmer2 = new TestableProgrammer("Jane Smith", "06207654321", "jane@example.com", ProgrammerRole.Backend, true, projectManager);

            projectManager.SetEmployees(new List<Programmer> { programmer1, programmer2 });

            var mockData = projectManager;

            _mockRepo.Setup(repo => repo.GetProjectManagerAsync(projectManager.Id)).ReturnsAsync(mockData);

            var result = await _service.GetProjectManagerAsync(projectManager.Id);
            result.Should().NotBeNull();

            result.name.Should().Be("Alice Johnson");
            result.phone.Should().Be("06101234567");
            result.email.Should().Be("alice@gmail.com");
            result.projects.Should().BeEmpty();

            result.employees.Should().HaveCount(2);
            result.employees[0].programmerName.Should().Be("John Doe");
            result.employees[0].programmerPhone.Should().Be("06201234567");
            result.employees[0].programmerEmail.Should().Be("john@example.com");
            result.employees[0].programmerRole.Should().Be(ProgrammerRole.FullStack);
            result.employees[0].programmerIsIntern.Should().BeFalse();

            result.employees[1].programmerName.Should().Be("Jane Smith");
            result.employees[1].programmerPhone.Should().Be("06207654321");
            result.employees[1].programmerEmail.Should().Be("jane@example.com");
            result.employees[1].programmerRole.Should().Be(ProgrammerRole.Backend);
            result.employees[1].programmerIsIntern.Should().BeTrue();
        }

        [Fact]
        public async Task GetProjectManagerByNotExistingProjectManagerId_Returns404Error()
        {
            var notExistingId = Guid.NewGuid();
            var mockData = (TestableProjectManager?)null;

            _mockRepo.Setup(repo => repo.GetProjectManagerAsync(notExistingId)).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProjectManagerAsync(notExistingId))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
        }
    }
}
