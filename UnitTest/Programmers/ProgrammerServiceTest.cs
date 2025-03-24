using Application.Commons;
using Application.Commons.DTOs;
using Application.Programmers;
using Application.Programmers.DTOs;
using Application.Programmers.Specs;
using Application.ProjectManagers;
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
            var result = await _service.ListProgrammersAsync();

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

            var result = await _service.ListProgrammersAsync();

            result.Should().BeEmpty();
        }

        /*--------------------------------------------------------Get-------------------------------------------------------*/
        [Fact]
        public async Task GetProgrammerById_ReturnsAvailableProgrammerWithAddressAndOneProjectAndWithoutProjectManager()
        {
            var programmerAddress = new TestableAddress("Hungary", "6722", "Csongrád", "Szeged", "Kossuth Lajos sugárút", "15.", 1);
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
            result.address.county.Should().Be("Csongrád");
            result.address!.settlement.Should().Be("Szeged");
            result.address!.street.Should().Be("Kossuth Lajos sugárút");
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
            mockSpec.Setup(spec => spec.ToExpressAll()).Returns(p => !p.IsArchived);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<Specification<Programmer>>())).ReturnsAsync(mockData);

            await FluentActions
                .Invoking(() => _service.GetProgrammerAsync(programmer.Id))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ErrorMessages.NOT_FOUND_PROGRAMMER);
        }

        /*--------------------------------------------------------Create-------------------------------------------------------*/
        [Theory]
        [InlineData(false, false, false)] // success case
        [InlineData(true, false, false)]  // email is already taken
        [InlineData(false, true, false)]  // project manager not found
        [InlineData(false, false, true)]  // success with valid Project Manager
        public async Task CreateProgrammerAsync_HandlesDifferentScenarios(bool isEmailTaken, bool isPmNotFound, bool isPmValid)
        {
            var programmerEmail = "test@example.com";
            var projectManagerId = isPmValid ? Guid.NewGuid() : (isPmNotFound ? Guid.NewGuid() : (Guid?)null);

            var dto = new ProgrammerCreateUpdateDTO
            {
                name = "Test Programmer",
                email = programmerEmail,
                phone = "06201234567",
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
                dateOfBirth = new DateOnly(1995, 5, 21),
                role = ProgrammerRole.FullStack,
                isIntern = false,
                projectManagerId = projectManagerId
            };

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerEmailSpec>()))
                .ReturnsAsync(isEmailTaken ? new TestableProgrammer("Existing", "06201234567", programmerEmail, ProgrammerRole.Backend, false) : null);

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<ProjectManagerIdSpec>()))
                .ReturnsAsync(isPmNotFound ? null : (isPmValid ? new TestableProjectManager("Alice Johnson", "06101234567", "alice@gmail.com") : null));

            if (isEmailTaken)
            {
                await FluentActions.Invoking(() => _service.CreateProgrammerAsync(dto))
                    .Should()
                    .ThrowAsync<BadRequestException>()
                    .WithMessage(ErrorMessages.TAKEN_PROGRAMMER_EMAIL);
            }
            else if (isPmNotFound)
            {
                await FluentActions.Invoking(() => _service.CreateProgrammerAsync(dto))
                    .Should()
                    .ThrowAsync<NotFoundException>()
                    .WithMessage(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }
            else
            {
                await _service.CreateProgrammerAsync(dto);

                _mockProgrammerRepo.Verify(repo => repo.CreateProgrammerAsync(It.IsAny<Programmer>()), Times.Once);
                _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
        }
        /*--------------------------------------------------------Update-------------------------------------------------------*/
        [Theory]
        [InlineData(false, false, false, false)] // success without pm
        [InlineData(true, false, false, false)]  // email is taken
        [InlineData(false, true, false, false)]  // programmer not found
        [InlineData(false, false, true, false)]  // pm not found
        [InlineData(false, false, false, true)]  // success with pm
        public async Task UpdateProgrammerAsync_HandlesDifferentScenarios(bool isEmailTaken, bool isProgrammerNotFound, bool isPmNotFound, bool isPmValid)
        {
            var programmerId = Guid.NewGuid();
            var existingEmail = "existing@example.com";
            var newEmail = "new@example.com";

            var existingProgrammer = isProgrammerNotFound
                ? null
                : new TestableProgrammer("Test Programmer", "06201234567", existingEmail, ProgrammerRole.FullStack, false, 
                    new TestableAddress("Hungary", "6722", "Csongrád", "Szeged", "Kossuth Lajos sugárút", "15.", 1));

            var dto = new ProgrammerCreateUpdateDTO
            {
                name = "Updated Programmer",
                email = isEmailTaken ? newEmail : existingEmail,
                phone = "06209998877",
                address = new AddressDTO
                {
                    country = "Hungary",
                    zipCode = "6722",
                    county = "Csongrád",
                    settlement = "Szeged",
                    street = "Petõfi Sándor utca",
                    houseNumber = "20.",
                    door = 2
                },
                dateOfBirth = new DateOnly(1992, 8, 14),
                role = ProgrammerRole.Backend,
                isIntern = true,
                projectManagerId = isPmValid ? Guid.NewGuid() : (isPmNotFound ? Guid.NewGuid() : (Guid?)null)
            };

            var existingEmailProgrammer = isEmailTaken ? new TestableProgrammer("Existing Programmer", "06209876543", newEmail, ProgrammerRole.Frontend, false) : null;
            
            var projectManager = isPmNotFound ? null : (isPmValid ? new TestableProjectManager("Project Manager", "06101234567", "pm@example.com") : null);

            _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerIdSpec>()))
                .ReturnsAsync(existingProgrammer);

            if (existingProgrammer is not null && dto.email != existingProgrammer.Email)
            {
                _mockProgrammerRepo.Setup(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerEmailSpec>()))
                    .ReturnsAsync(existingEmailProgrammer);
            }

            _mockProjectManagerRepo.Setup(repo => repo.GetProjectManagerAsync(It.IsAny<ProjectManagerIdSpec>()))
                .ReturnsAsync(projectManager);

            if (isProgrammerNotFound)
            {
                await FluentActions.Invoking(() => _service.UpdateProgrammerAsync(programmerId, dto))
                    .Should()
                    .ThrowAsync<NotFoundException>()
                    .WithMessage(ErrorMessages.NOT_FOUND_PROGRAMMER);
            }
            else if (isEmailTaken)
            {
                await FluentActions.Invoking(() => _service.UpdateProgrammerAsync(programmerId, dto))
                    .Should()
                    .ThrowAsync<BadRequestException>()
                    .WithMessage(ErrorMessages.TAKEN_PROGRAMMER_EMAIL);
            }
            else if (isPmNotFound)
            {
                await FluentActions.Invoking(() => _service.UpdateProgrammerAsync(programmerId, dto))
                    .Should()
                    .ThrowAsync<NotFoundException>()
                    .WithMessage(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }
            else
            {
                await _service.UpdateProgrammerAsync(programmerId, dto);

                _mockProgrammerRepo.Verify(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerIdSpec>()), Times.Once);
                if (existingProgrammer is not null && dto.email != existingProgrammer.Email)
                {
                    _mockProgrammerRepo.Verify(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerEmailSpec>()), Times.Once);
                }
                else
                {
                    _mockProgrammerRepo.Verify(repo => repo.GetProgrammerAsync(It.IsAny<ProgrammerEmailSpec>()), Times.Never);
                }
                _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
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