using Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.Projects.DTOs
{
    public class ProjectCreateUpdateDTO
    {
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROJECT_PROJECT_MANAGER)]
        public Guid projectManagerId { get; set; }
        public List<Guid> programmerIds { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROJECT_CUSTOMER)]
        public Guid customerId { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROJECT_DESCRIPTION)]
        [MaxLength(10000, ErrorMessage = ErrorMessages.TOO_LONG_PROJECT_DESCRIPTION)]
        public string description { get; set; }
    }
}
