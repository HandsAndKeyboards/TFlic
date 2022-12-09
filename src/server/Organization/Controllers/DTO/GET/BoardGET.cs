namespace Organization.Controllers.DTO.GET;

public class BoardGET
{
    public BoardGET(Models.Organization.Project.Board board)
    {
        Id = board.id;
        Name = board.Name;
        foreach (var column in board.Columns) { Columns.Add(column.Id); }
    }
    
    public ulong Id { get; set; }
    public string Name { get; set; }
    public List<ulong> Columns { get; } = new();
}