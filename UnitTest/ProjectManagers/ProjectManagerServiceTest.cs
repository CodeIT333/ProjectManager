using Application.Commons;
using Application.Commons.DTOs;
using Application.Programmers;
using Application.ProjectManagers;
using Application.ProjectManagers.DTOs;
using Application.ProjectManagers.Specs;
using Application.Projects;
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
using UnitTest.Projects;

namespace UnitTest.ProjectManagers
{
    public class ProjectManagerServiceTest
    {
        private readonly Mock<IProjectManagerRepository> _mockProjectManagerRepo;
        private readonly Mock<IProgrammerRepository> _mockProgrammerRepo;
        private readonly Mock<IProjectRepository> _mockProjectRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProjectManagerService _service;

        public ProjectManagerServiceTest()
        {
            TestMapsterConfig.Configure();
            _mockProjectManagerRepo = new Mock<IProjectManagerRepository>();
            _mockProgrammerRepo = new Mock<IProgrammerRepository>();
            _mockProjectRepo = new Mock<IProjectRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new ProjectManagerService(
                _mockProjectManagerRepo.Object, 
                _mockProgrammerRepo.Object, 
                _mockProjectRepo.Object, 
                _mockUnitOfWork.Object);
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
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.ListProjectManagersAsync(It.IsAny<Specification<ProjectManager>>()))
                .ReturnsAsync(mockData);

            var result = await _service.ListProjectManagersAsync(true);

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
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.ListProjectManagersAsync(It.IsAny<Specification<ProjectManager>>()))
                .ReturnsAsync(mockData);

            var result = await _service.ListProjectManagersAsync(true);

            result.Should().BeEmpty();
        }

        /*--------------------------------------------------------Get-------------------------------------------------------*/
        [Fact]
        public async Task GetProjectManagerById_ReturnsProjectManagerWithAddressAndProjectsAndWithoutEmployees()
        {
            var projectManagerAddress = new TestableAddress("Hungary", "6722", "Csongrád", "Szeged", "Kossuth Lajos sugárút", "15.", 1);
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com", new DateOnly(1990, 1, 1), projectManagerAddress);

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

            project1.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject1, programmerProject2 });
            project2.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject3 });

            projectManager.SetProjects(new List<Project> { project1, project2 });

            var mockData = projectManager;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == projectManager.Id && !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(mockData);

            var result = await _service.GetProjectManagerAsync(projectManager.Id);
            result.Should().NotBeNull();

            result.name.Should().Be("Alice Johnson");
            result.phone.Should().Be("06101234567");
            result.email.Should().Be("alice@gmail.com");
            result.dateOfBirth.Should().Be(new DateOnly(1990, 1, 1));
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
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == projectManager.Id && !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(mockData);

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
        public async Task GetProjectManagerByNotExistingProjectManagerId_Returns404ProjectManagerError()
        {
            var notExistingId = Guid.NewGuid();
            var mockData = (TestableProjectManager?)null;

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(new ProjectManagerIdSpec(notExistingId))).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProjectManagerAsync(notExistingId))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
        }

        /*--------------------------------------------------------Create-------------------------------------------------------*/
        [Fact]
        public async Task CreateProjectManagerWithEmployees_ReturnsOk()
        {
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var dto = new ProjectManagerCreateDTO
            {
                name = "New PM",
                email = "pm@gmail.com",
                phone = "06101234567",
                address = new AddressDTO
                {
                    country = "Hungary",
                    zipCode = "6722",
                    county = "Csongrád",
                    settlement = "Szeged",
                    street = "Kossuth Lajos sugárút",
                    houseNumber = "15.",
                    door = 1
                },
                dateOfBirth = new DateOnly(1980, 8, 15),
                employeeIds = new List<Guid> { programmer.Id }
            };

            var programmerWithTakenEmailMockSpec = new Mock<ISpecification<Programmer>>();
            programmerWithTakenEmailMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Email == dto.email && !p.IsArchived);
            var projectMockSpec = new Mock<ISpecification<Project>>();
            projectMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == p.Id && !p.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync((ProjectManager?)null);
            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(programmer);

            await _service.CreateProjectManagerAsync(dto);

            programmer.ProjectManager.Should().NotBeNull();
            programmer.ProjectManager.Name.Should().Be(dto.name);
            programmer.ProjectManager.Email.Should().Be(dto.email);
            programmer.ProjectManager.Phone.Should().Be(dto.phone);
            programmer.ProjectManager.DateOfBirth.Should().Be(dto.dateOfBirth);
            programmer.ProjectManager.IsArchived.Should().Be(false);
            programmer.ProjectManager.Address.Street.Should().Be(dto.address.street);
            programmer.ProjectManager.Employees.Should().Contain(programmer);

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /*--------------------------------------------------------Update-------------------------------------------------------*/
        [Fact]
        public async Task UpdateProjectManagerWithProgrammer_ReturnsOk()
        {
            var newProgrammer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "email@gmail.com", new DateOnly(1996,11,13), 
                new TestableAddress("Hungary", "6722", "Csongrád", "Szeged", "Bükk Kálmán utca", "26/B", 12));
            var previousProgrammer = new TestableProgrammer("Previous", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, projectManager);
            projectManager.SetEmployees(new List<Programmer> { previousProgrammer });
            var newProject = new TestableProject(
                null,
                new TestableCustomer("Acme Corp", "00000", "acme@gmail.com"),
                new DateOnly(2025, 03, 22),
                "Previous project");
            var previousProject = new TestableProject(
                projectManager,
                new TestableCustomer("New Corp", "00000", "new@gmail.com"),
                new DateOnly(2025, 03, 22),
                "New project");
            projectManager.SetProjects(new List<Project> { previousProject });
            var dto = new ProjectManagerUpdateDTO
            {
                name = "New Name",
                email = "email@gmail.com",
                phone = "0000000999",
                address = new AddressDTO
                {
                    country = "Hungary",
                    zipCode = "6722",
                    county = "Csongrád",
                    settlement = "Szeged",
                    street = "Kossuth Lajos sugárút",
                    houseNumber = "15.",
                    door = 1
                },
                dateOfBirth = new DateOnly(1980, 8, 15),
                projectIds = new List<Guid> { newProject.Id },
                employeeIds = new List<Guid> { newProgrammer.Id }
            };

            var pmMockSpec = new Mock<ISpecification<ProjectManager>>();
            pmMockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == projectManager.Id && !pm.IsArchived);
            var projectMockSpec = new Mock<ISpecification<Project>>();
            projectMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == newProject.Id && !newProject.IsArchived);
            var programmerMockSpec = new Mock<ISpecification<Programmer>>();
            programmerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == newProgrammer.Id && !newProgrammer.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(projectManager);
            _mockProjectRepo.Setup(repo => repo.GetProjectAsync(It.IsAny<Specification<Project>>())).ReturnsAsync(newProject);
            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(newProgrammer);

            await _service.UpdateProjectManagerAsync(projectManager.Id, dto);

            projectManager.Name.Should().Be("New Name");
            projectManager.Address.Street.Should().Be("Kossuth Lajos sugárút");
            projectManager.DateOfBirth.Should().Be(new DateOnly(1980, 8, 15));
            projectManager.Employees.Should().Contain(newProgrammer);
            projectManager.Employees.Should().NotContain(previousProgrammer);
            projectManager.Projects.Should().Contain(newProject);
            projectManager.Projects.Should().NotContain(previousProject);

            previousProgrammer.ProjectManager.Should().BeNull();
            previousProgrammer.ProjectManager.Should().NotBe(projectManager);
            newProgrammer.ProjectManager.Should().Be(projectManager);

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        
        /*--------------------------------------------------------Delete-------------------------------------------------------*/
        [Fact]
        public async Task DeleteProjectManagerWithoutEmployeesAndProjectRelation_ReturnsOk()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "email@gmail.com");

            var mockData = projectManager;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == projectManager.Id && !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(mockData);

            await _service.DeleteProjectManagerAsync(projectManager.Id);

            projectManager.IsArchived.Should().BeTrue();

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectManagerWithEmployeesRelation_ReturnsOk()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "email@gmail.com");
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, projectManager);
 
            projectManager.SetEmployees(new List<Programmer> { programmer });

            var mockData = projectManager;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == projectManager.Id && !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(mockData);

            await _service.DeleteProjectManagerAsync(projectManager.Id);

            projectManager.IsArchived.Should().BeTrue();
            projectManager.Employees.Should().BeEmpty();
            projectManager.Projects.Should().BeEmpty();

            programmer.ProjectManager.Should().BeNull();
            programmer.ProjectManagerId.Should().BeNull();

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectManagerWithProjectRelation_Returns400Exception()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "email@gmail.com");
            var project = new TestableProject(
                projectManager,
                new TestableCustomer("Acme Corp", "00000", "acme@gmail.com"),
                new DateOnly(2025, 03, 22),
                "New project");
            projectManager.SetProjects(new List<Project> { project });

            var mockData = projectManager;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == projectManager.Id && !pm.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.DeleteProjectManagerAsync(projectManager.Id))
                .Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ErrorMessages.EXISTING_PROJECT_FOR_PROJECT_MANAGER);
        }

        [Fact]
        public async Task DeleteProjectManager_Returns404sError()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "email@gmail.com", true);

            var mockData = (TestableProjectManager?)null;
            var mockSpec = new Mock<ISpecification<ProjectManager>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == projectManager.Id && !p.IsArchived);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.DeleteProjectManagerAsync(projectManager.Id))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
        }
    }
}
