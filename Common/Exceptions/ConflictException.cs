namespace Common.Exceptions;

public class ConflictException : Exception
{
    public List<string> Errors { get; }

    public ConflictException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ConflictException(List<string> errors) : base("Conflict")
    {
        Errors = errors;
    }
}
