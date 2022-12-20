using TFlic.Model.Authentication;

namespace TFlic.Model.Service;

/// <summary>
/// Хранимый на устройстве пользователя аккаунт
/// </summary>
public class StoredAccount
{
    public ulong Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public TokenPair Tokens { get; set; } = new();
}