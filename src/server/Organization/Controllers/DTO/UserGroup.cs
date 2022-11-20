namespace Organization.Controllers.DTO;

public record struct UserGroup
{
    public UserGroup(Models.Organization.Accounts.UserGroup userGroup)
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
