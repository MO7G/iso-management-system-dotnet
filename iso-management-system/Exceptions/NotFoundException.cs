using System;
using iso_management_system.Constants;

namespace iso_management_system.Exceptions;

// NotFoundException.cs
public class NotFoundException : Exception
{
    public ApiStatusCode StatusCode { get; } = ApiStatusCode.NotFound;

    public NotFoundException(string message) : base(message)
    {
    }
}
