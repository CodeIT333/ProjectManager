using Application.Commons.DTOs;
using Domain.Programmers;
using Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.Programmers.DTOs
{
    public class ProgrammerCreateUpdateDTO
    {
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_NAME)]
        [MaxLength(200, ErrorMessage = ErrorMessages.TOO_LONG_PROGRAMMER_NAME)]
        public string name { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_EMAIL)]
        [MaxLength(100, ErrorMessage = ErrorMessages.TOO_LONG_PROGRAMMER_EMAIL)]
        public string email { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_PHONE)]
        [MaxLength(20, ErrorMessage = ErrorMessages.TOO_LONG_PROGRAMMER_PHONE)]
        public string phone { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_ADDRESS)]
        public AddressDTO address { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_DATE_OF_BIRTH)]
        public DateOnly dateOfBirth { get; set; }
        public Guid? projectManagerId { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_ROLE)]
        public ProgrammerRole role { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_PROGRAMMER_IS_INTERN)]
        public bool isIntern { get; set; }
    }
}
