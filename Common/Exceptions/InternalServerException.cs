namespace Common.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException(string message = "An unexpected error occurred.") : base(message) { }
}
