namespace Application.Projects.DTOs
{
    public class ProjectInProgrammerGetDTO
    {
        public Guid id { get; set; }
        public string projectManagerName { get; set; }
        public DateOnly startDate { get; set; }
        public string description { get; set; }
    }
}
