namespace Organization.Controllers.DTO.GET;

public class ColumnGET
{
    public ColumnGET(Models.Organization.Project.Column column)
    {
        Id = column.Id;
        Position = column.Position;
        Name = column.Name;
        LimitOfTask = column.LimitOfTask;
        
        foreach (var task in column.Tasks) { Tasks.Add(task.Id); }
    }
    
    public ulong Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
    public int LimitOfTask { get; set; }
    public List<ulong> Tasks { get; } = new();
}