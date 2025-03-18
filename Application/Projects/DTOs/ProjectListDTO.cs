using Application.Commons.DTOs;

namespace Application.Projects.DTOs
{
    public class ProjectListDTO
    {
        public string projectManagerName { get; set; }
        public string customerName { get; set; }
        public List<string> programmerNames { get; set; }
        public DateOnly startDate { get; set; }
    }
}
