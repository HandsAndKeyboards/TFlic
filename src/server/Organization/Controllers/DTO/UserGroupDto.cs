namespace Organization.Controllers.DTO;

public record UserGroupDto
{
    public UserGroupDto(Models.Organization.Accounts.UserGroup userGroup)
    {
        GlobalId = userGroup.GlobalId;
        LocalId = userGroup.LocalId;
        OrganizationId = userGroup.OrganizationId;
        Name = userGroup.Name;
        foreach (var account in userGroup.Accounts) { Accounts.Add(account.Id); }
    }
    
    public ulong GlobalId { get; }
    public short LocalId { get; }
    public ulong OrganizationId { get; }
    public string Name { get; }
    public List<ulong> Accounts { get; } = new();
}
