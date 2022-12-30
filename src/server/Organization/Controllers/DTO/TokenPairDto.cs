namespace Organization.Controllers.DTO;

public record TokenPairDto
{
    public TokenPairDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; } 
    public string RefreshToken { get; }
}