namespace iso_management_system.Constants;

using System.Net;

public enum ApiStatusCode
{
    Ok = (int)HttpStatusCode.OK,
    Created = (int)HttpStatusCode.Created,
    BadRequest = (int)HttpStatusCode.BadRequest,
    Unauthorized = (int)HttpStatusCode.Unauthorized,
    Forbidden = (int)HttpStatusCode.Forbidden,
    NotFound = (int)HttpStatusCode.NotFound,
    Conflict = (int)HttpStatusCode.Conflict,
    InternalServerError = (int)HttpStatusCode.InternalServerError
}
