using Domain.Commons;
using Domain.Programmers;
using Domain.Projects;

namespace Application.Programmers.DTOs
{
    public class ProgrammerListDTO
    {
        public string name { get; set; }
        public Address address { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public List<Project> projects { get; set; } = [];
        public ProgrammerRole role { get; set; }
        public bool isIntern { get; set; }
    }
}
