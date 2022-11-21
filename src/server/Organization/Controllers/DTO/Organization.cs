namespace Organization.Controllers.DTO;

public record struct Organization
{
    public Organization(Models.Organization.Organization organization)
    {
        Id = organization.Id;
        Name = organization.Name;
        Description = organization.Description;
        foreach (var project in organization.Projects)
            if(project.is_archived)
                ArchiverProjects.Add(project.id);
            else
                ActiveProjects.Add(project.id);
    }
    
    public ulong Id { get; }
    public string Name { get; }
    public string? Description { get; }
    /*
     * todo при добавлении булевого флага "IsActive" в проект нужно заменить два 
     * todo массива "ActiveProjects" "ArchivedProjects" на один "Projects"
     */
    public List<ulong> ActiveProjects { get; } = new();
    public List<ulong> ArchiverProjects { get; } = new();
}