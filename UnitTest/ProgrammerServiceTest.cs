using Application.Programmers;
using Domain.Programmers;
using FluentAssertions;
using Moq;

namespace UnitTest
{
    public class ProgrammerServiceTest : Programmer
    {
        private readonly Mock<IProgrammerRepository> _mockRepo; // for mocking the repository
        private readonly ProgrammerService _service;  // inject the mock into the service

        public ProgrammerServiceTest()
        {
            _mockRepo = new Mock<IProgrammerRepository>();
            _service = new ProgrammerService(_mockRepo.Object);
        }

        [Fact]
        public void ListProgrammers_ReturnsListOfProgrammers()
        {
            //var mockData = new List<Programmer>
            //{
            //    new Programmer 
            //    { 
            //        Name = "John Doe", 
            //        Phone = "1234", 
            //        Email = "john@example.com", 
            //        Role = ProgrammerRole.FullStack, 
            //        IsIntern = false 
            //    },
            //    new Programmer 
            //    { 
            //        Name = "Jane Smith", 
            //        Phone = "5678", 
            //        Email = "jane@example.com", 
            //        Role = ProgrammerRole.Backend, 
            //        IsIntern = true }
            //};

            //_mockRepo.Setup(repo => repo.ListProgrammersAsync()).ReturnsAsync(mockData);

            //var result = await _service.ListProgrammersAsync();

            //result.Should().NotBeNull();
            //result.Should().HaveCount(2);
            //result[0].Name.Should().Be("John Doe");
            //result[1].Name.Should().Be("Jane Smith");
        }
    }
}