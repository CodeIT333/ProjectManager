using Application.Commons.DTOs;

namespace Application.Projects.DTOs
{
    public class ProjectInProjectManagerGetDTO : CustomerDTO
    {
        public Guid projectId { get; set; }
        public string projectDescription { get; set; }
    }
}
