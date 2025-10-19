namespace iso_management_system.Exceptions;

public class CustomValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public CustomValidationException(string message, Dictionary<string, string[]> errors = null)
        : base(message)
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }
}