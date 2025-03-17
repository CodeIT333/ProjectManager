namespace Application.Commons.DTOs
{
    public class AddressDTO
    {
        // TODO validations (swaggervalidation?)
        public string country { get; set; }
        public string zipCode { get; set; }
        public string county { get; set; }
        public string settlement { get; set; }
        public string street { get; set; }
        public string houseNumber { get; set; }
        public int? door { get; set; }
    }
}
