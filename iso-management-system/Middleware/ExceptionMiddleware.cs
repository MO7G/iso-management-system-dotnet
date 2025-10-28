using iso_management_system.Constants;
using iso_management_system.Exceptions;
using iso_management_system.Shared;

namespace iso_management_system.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly bool _showFullExceptionDetails; // <-- add this flag

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger,
        IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _showFullExceptionDetails = config.GetValue<bool>("AppSettings:ShowFullExceptionDetails");
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (_showFullExceptionDetails)
                // Development mode — log message and stack trace explicitly
                _logger.LogError("Unhandled exception: {Message} | {StackTrace}",
                    ex.GetBaseException().Message,
                    ex.GetBaseException().StackTrace);
            else
                // Production mode — only log short message
                _logger.LogError("Unhandled exception: {Message}", ex.GetBaseException().Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        ApiStatusCode statusCode;
        object responseBody;

        switch (ex)
        {
            case NotFoundException notFoundEx:
                statusCode = notFoundEx.StatusCode;
                responseBody = new ApiResponseWrapper<object>(statusCode, notFoundEx.Message);
                break;

            case BadRequestException badRequestEx:
                statusCode = badRequestEx.StatusCode;
                responseBody = new ApiResponseWrapper<object>(statusCode, badRequestEx.Message);
                break;

            case CustomValidationException validationEx:
                statusCode = validationEx.StatusCode;
                responseBody = new ApiResponseWrapper<Dictionary<string, string[]>>(
                    statusCode,
                    validationEx.Message,
                    validationEx.Errors
                );
                break;

            case BusinessRuleException businessRuleException: // <-- handle it here
                statusCode = businessRuleException.StatusCode; // 400
                responseBody = new ApiResponseWrapper<object>(statusCode, businessRuleException.Message);
                break;

            default:
                statusCode = ApiStatusCode.BadRequest;
                responseBody = new ApiResponseWrapper<object>(statusCode, "Internal Server Error");
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode; // convert from enum to int !!!
        return context.Response.WriteAsJsonAsync(responseBody);
    }
}