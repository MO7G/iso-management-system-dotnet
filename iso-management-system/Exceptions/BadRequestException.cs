namespace iso_management_system.Exceptions;


public class BadRequestException : Exception
{
    public int StatusCode { get; } = 400;

    public BadRequestException(string message) : base(message) { }
}