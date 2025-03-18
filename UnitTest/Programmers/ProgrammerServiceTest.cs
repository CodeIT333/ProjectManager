using Application.Programmers;
using Domain.Programmers;
using FluentAssertions;
using Moq;
using UnitTest.Commons;

namespace UnitTest.Programmers
{
    public class ProgrammerServiceTest : Programmer
    {
        private readonly Mock<IProgrammerRepository> _mockRepo; // for mocking the repository
        private readonly ProgrammerService _service;  // inject the mock into the service

        public ProgrammerServiceTest()
        {
            TestMapsterConfig.Configure(); // init mapster
            _mockRepo = new Mock<IProgrammerRepository>();
            _service = new ProgrammerService(_mockRepo.Object);
        }

        [Fact]
        public async Task ListProgrammers_ReturnsListOfProgrammers()
        {
            var mockData = new List<Programmer>
            {
                new TestableProgrammer("John Doe", "06201234567", "john@example.com", ProgrammerRole.FullStack, false),
                new TestableProgrammer("Jane Smith", "06207654321", "john@example.com", ProgrammerRole.Backend, true)
            };

            _mockRepo.Setup(repo => repo.ListProgrammersAsync()).ReturnsAsync(mockData);

            var result = await _service.ListProgrammersAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].name.Should().Be("John Doe");
            result[1].name.Should().Be("Jane Smith");
        }

        [Fact]
        public async Task ListProgrammers_ReturnsEmptyList()
        {
            var mockData = new List<Programmer>();

            _mockRepo.Setup(repo => repo.ListProgrammersAsync()).ReturnsAsync(mockData);

            var result = await _service.ListProgrammersAsync();

            result.Should().BeEmpty();
        }
    }
}