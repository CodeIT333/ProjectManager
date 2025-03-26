namespace Domain.Commons.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public int StatusCode { get; set; }
    }
}
