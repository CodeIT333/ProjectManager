using Domain.Programmers;

namespace Application.Programmers.DTOs
{
    public class ProgrammerInProjectManagerDTO
    {
        public Guid programmerId { get; set; }
        public string programmerName { get; set; }
        public string programmerEmail { get; set; }
        public string programmerPhone { get; set; }
        public ProgrammerRole programmerRole { get; set; }
        public bool programmerIsIntern { get; set; }
    }
}
