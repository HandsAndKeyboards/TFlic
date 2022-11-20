namespace Organization.Controllers.DTO;

public record struct Organization
{
    public Organization(Models.Organization.Organization organization)
    {
        Id = organization.Id;
        Name = organization.Name;
        Description = organization.Description;
        foreach (var activeProject in organization.ActiveProjects) { ActiveProjects.Add(activeProject.id); }
        foreach (var archivedProject in organization.ArchivedProjects) { ArchiverProjects.Add(archivedProject.id); }
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