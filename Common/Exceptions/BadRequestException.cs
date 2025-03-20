namespace Common.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }

    public BadRequestException(List<string> errors)
        : base("Bad Request")
    {
        Errors = errors;
    }

    public BadRequestException(string message, List<string> errors) : base(message)
    {
        Errors = errors;
    }

    public List<string>? Errors { get; set; }
}
