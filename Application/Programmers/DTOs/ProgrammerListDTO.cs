using Domain.Programmers;

namespace Application.Programmers.DTOs
{
    public class ProgrammerListDTO
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public ProgrammerRole role { get; set; }
        public bool isIntern { get; set; }
    }
}
