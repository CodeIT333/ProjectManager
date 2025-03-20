using Domain.Programmers;

namespace Application.Programmers.DTOs
{
    public class ProgrammerInProjectDTO
    {
        public Guid programmerId { get; set; }
        public string programmerName { get; set; }
        public ProgrammerRole programmerRole { get; set; }
        public bool programmerIsIntern { get; set; }
    }
}
