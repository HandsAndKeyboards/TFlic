namespace Organization.Controllers.DTO;

/// <summary>
/// Класс используется для аутентификации пользователя
/// </summary>
public record AuthenticationAccount
{
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
}