using iso_management_system.Constants;

namespace iso_management_system.Exceptions;

public class BusinessRuleException : Exception
{
    public ApiStatusCode StatusCode { get; } = ApiStatusCode.Conflict;

    public BusinessRuleException(string message) : base(message) { }
}