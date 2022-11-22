namespace Organization.Controllers.DTO;

public class Project
{
    public Project(Models.Organization.Project.Project project)
    {
        Id = project.id;
        Name = project.name;
        foreach (var board in project.boards) { Boards.Add(board.id); }
    }
    public ulong Id { get; set; }
    public string Name { get; set; }
    public List<ulong> Boards { get; } = new();
}