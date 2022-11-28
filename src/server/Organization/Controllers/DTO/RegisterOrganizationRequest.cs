namespace Organization.Controllers.DTO;

public record RegisterOrganizationRequest(string Name, string? Description, ulong CreatorId);