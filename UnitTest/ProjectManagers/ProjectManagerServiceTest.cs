using Application.ProjectManagers;
using Domain.ProjectManagers;
using FluentAssertions;
using Moq;
using UnitTest.Commons;

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
    }
}
