using System.ComponentModel.DataAnnotations;
using System.Net;
using iso_management_system.Exceptions;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Middleware;


public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        int statusCode;
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
                statusCode = 400;
                responseBody = new ApiResponseWrapper<Dictionary<string, string[]>>(
                    statusCode,
                    validationEx.Message,
                    validationEx.Errors
                );
                break;

            default:
                statusCode = 500;
                responseBody = new ApiResponseWrapper<object>(statusCode, "Internal Server Error");
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(responseBody);
    }
}

