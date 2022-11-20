namespace Organization.Controllers.DTO;

/// <summary>
/// Класс используется для регистрации пользователя в системе
/// </summary>
public record RegistrationAccount
{
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required string PasswordHash { get; set; }
}