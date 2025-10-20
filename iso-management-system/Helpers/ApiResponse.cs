using iso_management_system.Constants;
using iso_management_system.Shared;

namespace iso_management_system.Helpers;

public static class ApiResponse
{
    public static ApiResponseWrapper<T> Ok<T>(T data, string message = "Success") =>
        new ApiResponseWrapper<T>(ApiStatusCode.Ok, message, data);
    
    public static ApiResponseWrapper<T> Created<T>(T data, string message = "Created") =>
        new ApiResponseWrapper<T>(ApiStatusCode.Created, message, data);
    
}