using iso_management_system.Constants;
using iso_management_system.Exceptions;
using iso_management_system.Shared;

namespace iso_management_system.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly RequestDelegate _next; // represent the next middleware component in the http request 
    private readonly bool _showFullExceptionDetails;  

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger,
        IConfiguration config)
    {
        _next = next; 
        _logger = logger;
        _showFullExceptionDetails = config.GetValue<bool>("AppSettings:ShowFullExceptionDetails"); // from the app settings  json file
        
    }

    // we must call this method when the request reaches the middleware 
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // this continues the request chain if i don't call it the middleware will stop here and go down to the controller 
            // so we wait until the request comes back if there is an exception thrown we handle it here !!
            await _next(context); 
        }
        catch (Exception ex)
        {
            if (_showFullExceptionDetails)
                // development mode — log message and stack trace explicitly
                _logger.LogError("Unhandled exception: {Message} | {StackTrace}",
                    ex.GetBaseException().Message,
                    ex.GetBaseException().StackTrace);
            else
                // production mode — only log short message
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
        
        
        // Serialize the responseBody object to JSON and write it directly to the HTTP response stream.
        // This sends the JSON-formatted error response back to the client asynchronously.
        return context.Response.WriteAsJsonAsync(responseBody);
    }
}