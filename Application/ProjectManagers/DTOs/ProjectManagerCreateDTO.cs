using Application.Commons.DTOs;
using Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.ProjectManagers.DTOs
{
    public class ProjectManagerCreateDTO
    {
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_NAME)]
        [MaxLength(200, ErrorMessage = ErrorMessages.REQUIRED_PROJECT_MANAGER_NAME)]
        public string name { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_EMAIL)]
        [MaxLength(100, ErrorMessage = ErrorMessages.REQUIRED_PROJECT_MANAGER_EMAIL)]
        public string email { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_PHONE)]
        [MaxLength(20, ErrorMessage = ErrorMessages.REQUIRED_PROJECT_MANAGER_PHONE)]
        public string phone { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROJECT_MANAGER_ADDRESS)]
        public AddressDTO address { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROJECT_MANAGER_DATE_OF_BIRTH)]
        public DateOnly dateOfBirth { get; set; }
        public List<Guid>? employees { get; set; }
    }
}
