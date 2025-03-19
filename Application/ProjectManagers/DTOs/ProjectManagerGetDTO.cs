using Application.Commons.DTOs;
using Application.Programmers.DTOs;
using Application.Projects.DTOs;

namespace Application.ProjectManagers.DTOs
{
    public class ProjectManagerGetDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public AddressDTO address { get; set; }
        public DateTime dateOfBirth { get; set; }
        public List<ProjectInProjectManagerGetDTO> projects { get; set; }
        public List<ProgrammerInProjectManagerDTO> employees { get; set; }
    }
}
