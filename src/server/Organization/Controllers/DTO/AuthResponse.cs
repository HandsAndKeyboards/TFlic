namespace Organization.Controllers.DTO;

public record AuthResponse(Account Account, TokenPair Tokens);