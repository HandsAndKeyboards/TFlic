namespace Organization.Controllers.Service;

public static class TokenProvider
{
    public static string GetToken(HttpRequest request) =>
        request.Headers.Authorization.ToString().Replace("Bearer ", "");
}