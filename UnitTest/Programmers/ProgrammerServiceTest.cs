using Application.Commons;
using Application.Commons.DTOs;
using Application.Programmers;
using Application.Programmers.DTOs;
using Application.ProjectManagers;
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
using UnitTest.ProjectManagers;
using UnitTest.Projects;

namespace UnitTest.Programmers
{
    public class ProgrammerServiceTest : Programmer
    {
        private readonly Mock<IProgrammerProjectRepository> _mockProgrammerProjectRepo;
        private readonly Mock<IProjectManagerRepository> _mockProjectManagerRepo;
        private readonly Mock<IProgrammerRepository> _mockProgrammerRepo; // for mocking the repository
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProgrammerService _service;  // inject the mock into the service

        public ProgrammerServiceTest()
        {
            TestMapsterConfig.Configure(); // init mapster
            _mockProgrammerProjectRepo = new Mock<IProgrammerProjectRepository>();
            _mockProjectManagerRepo = new Mock<IProjectManagerRepository>();
            _mockProgrammerRepo = new Mock<IProgrammerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new ProgrammerService(
                _mockProgrammerProjectRepo.Object, 
                _mockProjectManagerRepo.Object, 
                _mockProgrammerRepo.Object, 
                _mockUnitOfWork.Object);
        }

        /*--------------------------------------------------------List-------------------------------------------------------*/
        [Fact]
        public async Task ListProgrammers_ReturnsListOfAvailableProgrammers()
        {
            var mockData = new List<Programmer>
            {
                new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false),
                new TestableProgrammer("Jane Smith", "06207654321", "jane@example.com", ProgrammerRole.Backend, true),
                new TestableProgrammer("Archived Programmer", "06207653333", "arch@example.com", ProgrammerRole.Backend, true, null, true),
            };

            // Create the mock specification to filter out archived programmers
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.ListProgrammersAsync(It.IsAny<Specification<Programmer>>()))
                .ReturnsAsync(mockData.Where(mockSpec.Object.ToExpressAll().Compile()).ToList());

            // Act
            var result = await _service.ListProgrammersAsync(true);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].name.Should().Be("John Doe");
            result[0].phone.Should().Be("06201234567");
            result[0].email.Should().Be("john@example.com");
            result[0].role.Should().Be(ProgrammerRole.FullStack);
            result[0].isIntern.Should().BeFalse();

            result[1].name.Should().Be("Jane Smith");
            result[1].phone.Should().Be("06207654321");
            result[1].email.Should().Be("jane@example.com");
            result[1].role.Should().Be(ProgrammerRole.Backend);
            result[1].isIntern.Should().BeTrue();
        }

        [Fact]
        public async Task ListProgrammers_ReturnsEmptyListOfAvailableProgrammers()
        {
            var mockData = new List<Programmer>();
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.ListProgrammersAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            var result = await _service.ListProgrammersAsync(true);

            result.Should().BeEmpty();
        }

        /*--------------------------------------------------------Get-------------------------------------------------------*/
        [Fact]
        public async Task GetProgrammerById_ReturnsAvailableProgrammerWithAddressAndOneProjectAndWithoutProjectManager()
        {
            var programmerAddress = new TestableAddress("Hungary", "6722", "Csongr�d", "Szeged", "Kossuth Lajos sug�r�t", "15.", 1);
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, programmerAddress);
            
            var customerForProject = new TestableCustomer("Acme Corp", "06501234566", "project@acme.com");
            
            var projectManagerForProject = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            
            var project = new TestableProject(
                projectManagerForProject,
                customerForProject,
                new DateOnly(2024, 3, 18),
                "Project description 1"
            );
            
            var programmerProject = new TestableProgrammerProject(programmer, project);
            project.SetProgrammerProjects(new List<ProgrammerProject> { programmerProject });

            programmer.ProgrammerProjects.Add(programmerProject);

            var mockData = programmer;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == programmer.Id && !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            var result = await _service.GetProgrammerAsync(programmer.Id);
            result.Should().NotBeNull();

            result.name.Should().Be("John Doe");
            result.phone.Should().Be("06201234567");
            result.email.Should().Be("john@example.com");
            result.role.Should().Be(ProgrammerRole.FullStack);
            result.isIntern.Should().BeFalse();

            result.address.Should().NotBeNull();
            result.address!.country.Should().Be("Hungary");
            result.address!.zipCode.Should().Be("6722");
            result.address.county.Should().Be("Csongr�d");
            result.address!.settlement.Should().Be("Szeged");
            result.address!.street.Should().Be("Kossuth Lajos sug�r�t");
            result.address!.houseNumber.Should().Be("15.");
            result.address!.door.Should().Be(1);

            result.projects.Should().HaveCount(1);
            result.projectManagerName.Should().BeNull();
            result.projects[0].projectManagerName.Should().Be("Alice Johnson");
            result.projects[0].startDate.Should().Be(new DateOnly(2024, 3, 18));
        }

        [Fact]
        public async Task GetProgrammerById_ReturnsAvailableProgrammerWithoutProjectAndWithProjectManager()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, projectManager);

            var mockData = programmer;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == programmer.Id && !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            var result = await _service.GetProgrammerAsync(programmer.Id);
            result.Should().NotBeNull();

            result.name.Should().Be("John Doe");
            result.phone.Should().Be("06201234567");
            result.email.Should().Be("john@example.com");
            result.role.Should().Be(ProgrammerRole.FullStack);
            result.isIntern.Should().BeFalse();
            result.projects.Should().HaveCount(0);
            result.projectManagerName.Should().Be("Alice Johnson");
        }

        [Fact]
        public async Task GetAvailableProgrammerByNotExistingProgrammerId_Returns404ProgrammerError()
        {
            var notExistingId = Guid.NewGuid();
            var mockData = (TestableProgrammer?)null;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == notExistingId && !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProgrammerAsync(notExistingId))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROGRAMMER);
        }

        [Fact]
        public async Task GetUnavailableProgrammerById_Returns404ProgrammerError()
        {
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, null, true);
            
            var mockData = (TestableProgrammer?)null;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == programmer.Id && !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProgrammerAsync(programmer.Id))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROGRAMMER);
        }

        /*--------------------------------------------------------Create-------------------------------------------------------*/
        [Fact]
        public async Task CreateProgrammerWithProjectManager_ReturnsOk()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var dto = new ProgrammerCreateUpdateDTO
            {
                name = "John Doe",
                email = "notTakenEmail@example.com",
                phone = "06201234567",
                role = ProgrammerRole.FullStack,
                isIntern = false,
                address = new AddressDTO
                {
                    country = "Hungary",
                    zipCode = "6722",
                    county = "Csongr�d",
                    settlement = "Sz�k�k�t",
                    street = "R�g� utca",
                    houseNumber = "33"
                },
                dateOfBirth = new DateOnly(2000, 06, 22),
                projectManagerId = projectManager.Id
            };

            var programmerMockSpec = new Mock<ISpecification<Programmer>>();
            programmerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Email == dto.email && !p.IsArchived);
            var projectManagerMockSpec = new Mock<ISpecification<ProjectManager>>();
            projectManagerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == dto.projectManagerId && !pm.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync((Programmer?)null);
            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(projectManager);

            await _service.CreateProgrammerAsync(dto);

            projectManager.Employees.Should().HaveCount(1);
            projectManager.Employees[0].Name.Should().Be(dto.name);
            projectManager.Employees[0].Email.Should().Be(dto.email);
            projectManager.Employees[0].Phone.Should().Be(dto.phone);
            projectManager.Employees[0].IsIntern.Should().Be(dto.isIntern);
            projectManager.Employees[0].IsArchived.Should().Be(false);
            projectManager.Employees[0].Address.Street.Should().Be(dto.address.street);
            projectManager.Employees[0].ProjectManagerId.Should().Be(dto.projectManagerId);

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateProgrammerWithTakenEmail_Returns400ProgrammerError()
        {
            var existingEmail = "existing@gmail.com";
            var programmerWithExistingEmail = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false,
                new TestableAddress("Hungary", "6722", "Csongr�d", "Szeged", "Kossuth Lajos sug�r�t", "26/B", 12));
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var dto = new ProgrammerCreateUpdateDTO
            {
                name = "John Doe",
                email = existingEmail,
                phone = "06201234567",
                role = ProgrammerRole.FullStack,
                isIntern = false,
                address = new AddressDTO
                {
                    country = "Hungary",
                    zipCode = "6722",
                    county = "Csongr�d",
                    settlement = "Sz�k�k�t",
                    street = "R�g� utca",
                    houseNumber = "33"
                },
                dateOfBirth = new DateOnly(2000, 06, 22),
                projectManagerId = projectManager.Id
            };

            var programmerMockSpec = new Mock<ISpecification<Programmer>>();
            programmerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Email == dto.email && !p.IsArchived);
            var projectManagerMockSpec = new Mock<ISpecification<ProjectManager>>();
            projectManagerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == dto.projectManagerId && !pm.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(programmerWithExistingEmail);
            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(projectManager);

            await FluentActions
                .Invoking(() => _service.CreateProgrammerAsync(dto))
                .Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ErrorMessages.TAKEN_PROGRAMMER_EMAIL);
        }

        /*--------------------------------------------------------Update-------------------------------------------------------*/
        [Fact]
        public async Task UpgradeProgrammerWithProjectManager_ReturnsOk()
        {
            var previousProjectManager = new TestableProjectManager("Old manager", "06101234567", "old@gmail.com");
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.Backend, false,
                new TestableAddress("Hungary", "6722", "Csongr�d", "Szeged", "Kossuth Lajos sug�r�t", "26/B", 12), previousProjectManager);
            previousProjectManager.SetEmployees(new List<Programmer> { programmer });
            var newProjectManager = new TestableProjectManager("New manager", "06101234567", "new@gmail.com");
            var dto = new ProgrammerCreateUpdateDTO
            {
                name = "New name",
                email = "new@example.com",
                phone = "06201234567",
                role = ProgrammerRole.FullStack,
                isIntern = false,
                address = new AddressDTO
                {
                    country = "Hungary",
                    zipCode = "6722",
                    county = "Csongr�d",
                    settlement = "Sz�k�k�t",
                    street = "R�g� utca",
                    houseNumber = "33"
                },
                dateOfBirth = new DateOnly(2000, 06, 22),
                projectManagerId = newProjectManager.Id
            };

            var programmerMockSpec = new Mock<ISpecification<Programmer>>();
            programmerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == programmer.Id && !p.IsArchived);
            var programmerWithTakenEmailMockSpec = new Mock<ISpecification<Programmer>>();
            programmerWithTakenEmailMockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Email == dto.email && !p.IsArchived);
            var projectManagerMockSpec = new Mock<ISpecification<ProjectManager>>();
            projectManagerMockSpec.Setup(spec => spec.ToExpressAll()).Returns(pm => pm.Id == dto.projectManagerId && !pm.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.Is<Specification<Programmer>>(s => s.ToExpressAll().Compile()(programmer)))).ReturnsAsync(programmer);
            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.Is<Specification<Programmer>>(s => s.ToExpressAll().Compile()(programmer) == false))).ReturnsAsync((Programmer?)null);
            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<Specification<ProjectManager>>())).ReturnsAsync(newProjectManager);

            await _service.UpdateProgrammerAsync(programmer.Id, dto);

            programmer.ProjectManager.Should().Be(newProjectManager);
            newProjectManager.Employees.Should().HaveCount(1);
            programmer.Name.Should().Be(dto.name);
            programmer.Email.Should().Be(dto.email);
            programmer.Phone.Should().Be(dto.phone);
            programmer.Role.Should().Be(dto.role);
            programmer.IsIntern.Should().Be(dto.isIntern);
            programmer.IsArchived.Should().Be(false);
            programmer.DateOfBirth.Should().Be(dto.dateOfBirth);
            programmer.Address.Street.Should().Be(dto.address.street);

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /*--------------------------------------------------------Delete-------------------------------------------------------*/
        [Fact]
        public async Task DeleteProgrammerWithNoPmAndProjectRelation_ReturnsOk()
        {
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false);

            var mockData = programmer;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == programmer.Id && !p.IsArchived);
            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);
            _mockProgrammerProjectRepo.Setup(repo => repo.DeleteProgrammerProject(It.IsAny<ProgrammerProject>())).Verifiable();

            await _service.DeleteProgrammerAsync(programmer.Id);

            programmer.IsArchived.Should().BeTrue();

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProgrammerWithProjectManagerAndProjectRelation_ReturnsOk()
        {
            var projectManager = new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com");
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, projectManager);
            var project = new TestableProject(
                projectManager, 
                new TestableCustomer("Acme Corp", "00000", "acme@gmail.com"), 
                new DateOnly(2025, 03, 22), 
                "New project");
            var programmerProjects = new List<ProgrammerProject>()
            {
                new TestableProgrammerProject(programmer, project)
            };
            project.SetProgrammerProjects(programmerProjects);
            programmer.SetProgrammerProjects(programmerProjects);
            projectManager.Employees.Add(programmer);

            var mockData = programmer;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => p.Id == programmer.Id && !p.IsArchived);
            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);
            _mockProgrammerProjectRepo.Setup(repo => repo.DeleteProgrammerProject(It.IsAny<ProgrammerProject>())).Verifiable();

            await _service.DeleteProgrammerAsync(programmer.Id);

            programmer.IsArchived.Should().BeTrue();
            programmer.ProjectManager.Should().BeNull();
            programmer.ProgrammerProjects.Should().BeEmpty();

            project.ProgrammerProjects.Should().BeEmpty();

            projectManager.Employees.Should().NotContain(programmer);

            _mockProgrammerProjectRepo.Verify(repo => repo.DeleteProgrammerProject(It.IsAny<ProgrammerProject>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProgrammer_Returns404sError()
        {
            var programmer = new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false, null, true);

            var mockData = (TestableProgrammer?)null;
            var mockSpec = new Mock<ISpecification<Programmer>>();
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.DeleteProgrammerAsync(programmer.Id))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROGRAMMER);
        }

    }
}