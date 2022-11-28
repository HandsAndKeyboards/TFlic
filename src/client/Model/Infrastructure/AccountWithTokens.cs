using TFlic.Model.DTO;

namespace TFlic.Model.Infrastructure;

public record AccountWithTokens(Account Account, TokenPair Tokens);