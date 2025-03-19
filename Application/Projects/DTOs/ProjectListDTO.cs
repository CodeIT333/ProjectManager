namespace Application.Projects.DTOs
{
    public class ProjectListDTO : ProjectInProgrammerGetDTO
    {
        public Guid id { get; set; }
        public string customerName { get; set; }
        public List<string> programmerNames { get; set; }
    }
}
