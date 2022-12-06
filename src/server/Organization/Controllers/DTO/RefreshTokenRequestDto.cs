namespace Organization.Controllers.DTO;

public record RefreshTokenRequestDto
{
    public RefreshTokenRequestDto(string refreshToken, string login)
    {
        RefreshToken = refreshToken;
        Login = login;
    }

    public string RefreshToken { get; }
    public string Login { get; }
}
