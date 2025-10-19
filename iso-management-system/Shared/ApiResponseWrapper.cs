namespace iso_management_system.Shared;

// ApiResponseWrapper.cs
public class ApiResponseWrapper<T>
{
    public int Status { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiResponseWrapper(int status, string message, T? data = default)
    {
        Status = status;
        Message = message;
        Data = data;
    }
}
