using Application.Customers.DTOs;
using Application.Programmers.DTOs;
using Application.ProjectManagers.DTOs;

namespace Application.Projects.DTOs
{
    public class ProjectGetDTO
    {
        public Guid id { get; set; }
        public List<ProgrammerInProjectDTO> programmers { get; set; }
        public ProjectManagerInProgrammerDTO projectManager { get; set; }
        public CustomerDTO customer { get; set; }
        public DateOnly startDate { get; set; }
        public string description { get; set; }
    }
}
