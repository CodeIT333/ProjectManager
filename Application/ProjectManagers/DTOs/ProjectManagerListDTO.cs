using Application.Commons.DTOs;

namespace Application.ProjectManagers.DTOs
{
    public class ProjectManagerListDTO : NameDTO
    {
        public Guid id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
