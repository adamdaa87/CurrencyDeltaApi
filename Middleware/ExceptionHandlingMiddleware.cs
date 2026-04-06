using System.Text.Json;
using CurrencyDeltaApi.Exceptions;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Middleware;

/// <summary>
/// Catches unhandled exceptions and converts them into consistent JSON error responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CurrencyValidationException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                new ErrorResponse(ex.ErrorCode, ex.ErrorDetails));
        }
        catch (ExternalApiException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status502BadGateway,
                new ErrorResponse("externalapierror", ex.Message));
        }
        catch (Exception ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError,
                new ErrorResponse("internalerror", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, ErrorResponse error)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsJsonAsync(error, options);
    }
}
