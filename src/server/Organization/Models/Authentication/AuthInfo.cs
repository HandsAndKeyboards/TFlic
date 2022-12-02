using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Authentication;

[Table("auth_info")]
public class AuthInfo
{
    /// <summary>
    /// Уникальный идентификатор аккаунта, к которому относится информация
    /// </summary>
    [Key]
    [Column("account_id")]
    public ulong AccountId { get; set; }
    
    /// <summary>
    /// Логин аккаунта
    /// </summary>
    [Column("login"), MaxLength(50)]
    public required string Login { get; init; }
    
    /// <summary>
    /// Хеш пароля
    /// </summary>
    /// <remarks>Хеш закодирован в base64</remarks>
    [Column("password_hash")]
    public required string PasswordHash { get; set; }
    
    /// <summary>
    /// Токен обновления
    /// </summary>
    [Column("refresh_token")]
    public string? RefreshToken { get; set; }

    public Account Account { get; set; } = null!;
}