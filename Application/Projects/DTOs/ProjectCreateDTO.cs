namespace Application.Projects.DTOs
{
    public class ProjectCreateDTO
    {
        public Guid projectManagerId { get; set; }
        public List<Guid> programmerIds { get; set; }
        public Guid customerId { get; set; }
        public string description { get; set; }
    }
}
