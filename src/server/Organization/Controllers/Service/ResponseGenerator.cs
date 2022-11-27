using Microsoft.AspNetCore.Mvc;

namespace Organization.Controllers.Service;

public static class ResponseGenerator
{
    public static IActionResult Ok(string? message = null, object? value = null) =>
        Generate<OkObjectResult>(message ?? "Success", value);
    
    
    public static IActionResult Unauthorized(string? message = null, object? value = null) =>
        Generate<UnauthorizedObjectResult>(message ?? "Unauthorized", value);
    
    public static IActionResult NotFound(string? message = null, object? value = null) =>
        Generate<NotFoundObjectResult>(message ?? "Not found", value);
    
    public static IActionResult Forbidden(string? message = null, object? value = null) => 
        Generate<ForbidResult>(message ?? "Access denied", value);
    
    public static IActionResult BadRequestResult(string? message = null, object? value = null) => 
        Generate<BadRequestObjectResult>(message ?? "Bad request", value);

    private static IActionResult Generate<TResult>(string message, object? value = null) 
        where TResult : IActionResult
    {
        var res = value is null
            ? Activator.CreateInstance(typeof(TResult), new {Message = message})
            : Activator.CreateInstance(typeof(TResult), new {Message = message, Value = value});
        
        return (TResult) res!;
    }
}