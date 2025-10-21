using System;
using iso_management_system.Constants;

namespace iso_management_system.Shared;

// ApiResponseWrapper.cs
public class ApiResponseWrapper<T>
{
    public int Status { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiResponseWrapper(ApiStatusCode statusCode, string message, T? data = default)
    {
        Status = (int)statusCode;
        Message = message;
        Data = data;
    }
}
