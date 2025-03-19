using Application.Commons.DTOs;
using Application.Projects.DTOs;
using Domain.Programmers;

namespace Application.Programmers.DTOs
{
    public class ProgrammerGetDTO : NameDTO
    {
        public Guid id { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public AddressDTO address { get; set; }
        public DateTime dateOfBirth { get; set; }
        public List<ProjectInProgrammerGetDTO> projects { get; set; } = [];
        public Guid? projectManagerId { get; set; }
        public string? projectManagerName { get; set; }
        public ProgrammerRole role { get; set; }
        public bool isIntern { get; set; }
    }
}
