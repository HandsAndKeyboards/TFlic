using System.Net;

namespace TFlic.Model.Infrastructure;

public class HttpResult
{
    public HttpResult(HttpStatusCode statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message;
    }
    
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}