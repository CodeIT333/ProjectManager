namespace Infrastructure.Exceptions
{
    public abstract class ProjectManagerAPIException : Exception
    {
        public int StatusCode { get; }
        public ProjectManagerAPIException(string message, int statusCode ) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
