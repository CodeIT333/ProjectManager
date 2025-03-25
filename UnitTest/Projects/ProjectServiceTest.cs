using Application.Commons;
using Application.Customers;
using Application.Programmers;
using Application.ProjectManagers;
using Application.Projects;
using Application.Projects.Specs;
using Domain.Commons;
using Domain.Programmers;
using Domain.ProjectManagers;
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
        private readonly Mock<IProgrammerProjectRepository> _mockProgrammerProjectRepo;
        private readonly Mock<IProjectManagerRepository> _mockProjectManagerRepo;
        private readonly Mock<IProgrammerRepository> _mockProgrammerRepo;
        private readonly Mock<ICustomerRepository> _mockCustomerRepo;
        private readonly Mock<IProjectRepository> _mockProjectRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProjectService _service;

        public ProjectServiceTest()
        {
            TestMapsterConfig.Configure();
            _mockProgrammerProjectRepo = new Mock<IProgrammerProjectRepository>();
            _mockProjectManagerRepo = new Mock<IProjectManagerRepository>();
            _mockProgrammerRepo = new Mock<IProgrammerRepository>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockProjectRepo = new Mock<IProjectRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new ProjectService(
                _mockProgrammerProjectRepo.Object, 
                _mockProjectManagerRepo.Object,
                _mockProgrammerRepo.Object,
                _mockCustomerRepo.Object,
                _mockProjectRepo.Object, 
                _mockUnitOfWork.Object);
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

            project1.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });
            project2.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject3 });

            var mockData = new List<Project> { project1, project2 };

            _mockProjectRepo.Setup(repo => repo.ListProjectsAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

            var result = await _service.ListProjectsAsync(true);

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
        public async Task ListProjects_ReturnsListOfArchivedProjects()
        {
            var project = new TestableProject(
                null,
                null,
                new DateOnly(2024, 3, 18),
                "Project description 1",
                true
            );

            var mockData = new List<Project> { project };

            _mockProjectRepo.Setup(repo => repo.ListProjectsAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);
            var result = await _service.ListProjectsAsync(false);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].customerName.Should().BeNull();
            result[0].projectManagerName.Should().BeNull();
            result[0].description.Should().Be("Project description 1");
        }

        [Fact]
        public async Task ListProjects_ReturnsEmptyList()
        {
            var mockData = new List<Project>();

            _mockProjectRepo.Setup(repo => repo.ListProjectsAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

            var result = await _service.ListProjectsAsync(true);

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
            project.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });

            var mockData = project;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == project.Id && !p.IsArchived);

            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

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

            var mockData = project;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == project.Id && !p.IsArchived);

            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

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

            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(new ProjectIdSpec(notExistingId))).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProjectAsync(notExistingId))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROJECT);
        }

        /*--------------------------------------------------------Create-------------------------------------------------------*/
        /*
        [Theory]
        [InlineData(false, false, false)] // success
        [InlineData(true, false, false)]  // project manager not found
        [InlineData(false, true, false)]  // customer not found
        [InlineData(false, false, true)]  // programmer not found
        public async Task CreateProjectAsync_HandlesDifferentScenarios(bool isPmNotFound, bool isCustomerNotFound, bool isProgrammerNotFound)
        {
            var projectManagerId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var validProgrammerIds = new List<Guid> { Guid.NewGuid() };

            var dto = new ProjectCreateUpdateDTO
            {
                description = "Project description",
                projectManagerId = projectManagerId,
                customerId = customerId,
                programmerIds = isProgrammerNotFound ? validProgrammerIds : new List<Guid>() { Guid.NewGuid() }
            };

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<ProjectManagerIdSpec>()))
                .ReturnsAsync(isPmNotFound ? null : new TestableProjectManager("Project Manager", "06101234567", "pm@example.com"));

            _mockCustomerRepo.Setup(repo => repo.GetCustomerAsync(customerId))
                .ReturnsAsync(isCustomerNotFound ? null : new TestableCustomer("Customer Name", "06501234567", "customer@example.com"));

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerIdSpec>()))
                .ReturnsAsync((ProgrammerIdSpec spec) =>
                {
                    return new TestableProgrammer("Test Programmer", "06201234567", "programmer@example.com", ProgrammerRole.FullStack, false);
                });

            if (isPmNotFound)
            {
                await FluentActions.Invoking(() => _service.CreateProjectAsync(dto))
                    .Should()
                    .ThrowAsync<NotFoundException>()
                    .WithMessage(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }
            else if (isCustomerNotFound)
            {
                await FluentActions.Invoking(() => _service.CreateProjectAsync(dto))
                    .Should()
                    .ThrowAsync<NotFoundException>()
                    .WithMessage(ErrorMessages.NOT_FOUND_CUSTOMER);
            }
            else
            {
                await _service.CreateProjectAsync(dto);

                _mockProjectRepo.Verify(repo => repo.CreateProjectAsync(It.IsAny<Project>()), Times.Once);
                _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                if (!isProgrammerNotFound)
                {
                    _mockProgrammerRepo.Verify(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerIdSpec>()), Times.Exactly(validProgrammerIds.Count));
                    _mockProgrammerProjectRepo.Verify(repo => repo.CreateProgrammerProjectAsync(It.IsAny<ProgrammerProject>()), Times.Exactly(validProgrammerIds.Count));
                }
            }
        }
        */

        /*--------------------------------------------------------Delete-------------------------------------------------------*/
        [Fact]
        public async Task DeleteProjectWithoutProjectManagerAndCustomerAndProgrammersRelation_ReturnsOk()
        {
            var project = new TestableProject(
                null,
                null,
                new DateOnly(2024, 3, 18),
                "Project without anything"
            );

            var mockData = project;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == project.Id && !p.IsArchived);

            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

            await _service.DeleteProjectAsync(project.Id);

            project.IsArchived.Should().BeTrue();
            project.Description.Should().Be("Project without anything");

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectWithProjectManagerAndCustomerAndProgrammersRelation_ReturnsOk()
        {
            var programmer1 = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var programmer2 = new TestableProgrammer("Jane Smith", "06207654321", "jane@example.com", ProgrammerRole.Backend, true);
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var customer = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");
            var project = new TestableProject(
                projectManager,
                customer,
                new DateOnly(2024, 3, 18),
                "Project"
            );
            var programmerProject1 = new TestableProgrammerProject(programmer1, project);
            var programmerProject2 = new TestableProgrammerProject(programmer2, project);
            programmer1.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject1 });
            programmer2.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject2 });
            project.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });

            var mockData = project;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == project.Id && !p.IsArchived);

            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

            await _service.DeleteProjectAsync(project.Id);

            project.IsArchived.Should().BeTrue();
            project.Customer.Should().BeNull();
            project.ProjectManager.Should().BeNull();
            project.ProgrammerProjects.Should().BeEmpty();
            project.Description.Should().Be("Project");

            programmer1.ProgrammerProjects.Should().BeEmpty();
            programmer2.ProgrammerProjects.Should().BeEmpty();

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProject_Returns404Exception()
        {
            var project = new TestableProject(
                null,
                null,
                new DateOnly(2024, 3, 18),
                "Project"
            );

            var mockData = (TestableProject?)null;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == project.Id && !p.IsArchived);

            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.DeleteProjectAsync(project.Id))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROJECT);
        }
    }
}
