namespace Organization.Controllers.DTO;

public record RegisterOrganizationRequestDto
{
    public RegisterOrganizationRequestDto(string name, string description, ulong creatorId)
    {
        Name = name;
        Description = description;
        CreatorId = creatorId;
    }

    public string Name { get; }
    public string Description { get; }
    public ulong CreatorId { get; }
}