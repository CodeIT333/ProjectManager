using Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.Commons.DTOs
{
    public class AddressDTO
    {
        [Required(ErrorMessage = ErrorMessages.REQUIRED_ADDRESS_COUNTRY)]
        [MaxLength(50, ErrorMessage = ErrorMessages.TOO_LONG_ADDRESS_COUNTRY)]
        public string country { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_ADDRESS_ZIP_CODE)]
        [MaxLength(10, ErrorMessage = ErrorMessages.TOO_LONG_ADDRESS_ZIP_CODE)]
        public string zipCode { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_ADDRESS_COUNTY)]
        [MaxLength(50, ErrorMessage = ErrorMessages.TOO_LONG_ADDRESS_COUNTY)]
        public string county { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_ADDRESS_SETTLEMENT)]
        [MaxLength(100, ErrorMessage = ErrorMessages.TOO_LONG_ADDRESS_SETTLEMENT)]
        public string settlement { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_ADDRESS_STREET)]
        [MaxLength(50, ErrorMessage = ErrorMessages.TOO_LONG_ADDRESS_STREET)]
        public string street { get; set; }
        [Required(ErrorMessage = ErrorMessages.REQUIRED_ADDRESS_HOUSE_NUMBER)]
        [MaxLength(10, ErrorMessage = ErrorMessages.TOO_LONG_ADDRESS_HOUSE_NUMBER)]
        public string houseNumber { get; set; }
        public int? door { get; set; }
    }
}
