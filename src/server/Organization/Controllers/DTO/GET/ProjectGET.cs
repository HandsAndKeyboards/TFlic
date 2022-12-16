namespace Organization.Controllers.DTO.GET;

public class ProjectGET
{
    public ProjectGET(Models.Organization.Project.Project project)
    {
        Id = project.id;
        Name = project.name;
        if (project.boards == null) return;
        foreach (var board in project.boards) { Boards.Add(board.id); }
    }
    public ulong Id { get; set; }
    public string Name { get; set; }
    public List<ulong> Boards { get; } = new();
}