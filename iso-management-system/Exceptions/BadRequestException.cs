using iso_management_system.Constants;

namespace iso_management_system.Exceptions;


public class BadRequestException : Exception
{
    public ApiStatusCode StatusCode { get; } = ApiStatusCode.BadRequest;

    public BadRequestException(string message) : base(message) { }
}