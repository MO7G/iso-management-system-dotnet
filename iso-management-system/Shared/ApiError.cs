namespace iso_management_system.Shared;


// ApiError.cs
public class ApiError
{
    public int Status { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiError(int status, string message, Dictionary<string, string>? errors = null)
    {
        Status = status;
        Message = message;
        Errors = errors;
    }
}

