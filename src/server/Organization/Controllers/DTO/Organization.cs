namespace Organization.Controllers.DTO;

public record Organization
{
    public Organization(Models.Organization.Organization organization)
    {
        Id = organization.Id;
        Name = organization.Name;
        Description = organization.Description;
        UserGroups = organization.GetUserGroups().Select(ug => new UserGroupIdSet(ug.GlobalId, ug.LocalId)).ToList();
        ActiveProjects = organization.Projects.Where(prj => !prj.is_archived).Select(ug => ug.id).ToList();
        ArchivedProjects = organization.Projects.Where(prj => prj.is_archived).Select(ug => ug.id).ToList();
    }
    
    public ulong Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public List<UserGroupIdSet> UserGroups { get; }
    /*
     * todo
     * Азим: при добавлении булевого флага "IsActive" в проект нужно заменить два 
     * todo массива "ActiveProjects" "ArchivedProjects" на один "Projects"
     */
    public List<ulong> ActiveProjects { get; }
    public List<ulong> ArchivedProjects { get; }
}

public record UserGroupIdSet(ulong GlobalId, short LocalId);