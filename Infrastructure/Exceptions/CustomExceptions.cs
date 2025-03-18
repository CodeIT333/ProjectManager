namespace Infrastructure.Exceptions
{
    public class NotFoundException : ProjectManagerAPIException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }

    public class BadRequestException : ProjectManagerAPIException
    {
        public BadRequestException(string message) : base(message, 400) { }
    }
}
