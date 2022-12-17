namespace Organization.Controllers.DTO.GET;

public class TaskGET
{
    public TaskGET(Models.Organization.Project.Task task)
    {
        Id = task.Id;
        IdColumn = task.ColumnId;
        Position = task.Position;
        Name = task.Name;
        CreationTime = task.CreationTime;
        Description = task.Description;
        Status = task.Status;
        id_executor = task.ExecutorId;
        Deadline = task.Deadline;
        Priority = task.priority;
        if (task.Components == null) return;
        foreach (var component in task.Components) { Components.Add(component.id); }
    }
    
    public ulong Id { get; init; }
    public ulong IdColumn { get; init; }

    public int Position { get; set; }
    public string Name { get; set; }

    public ulong Priority { get; init; }

    public string Description { get; set; }
    public ulong id_executor { get; set; }
    public List<ulong> Authors {get;} = new();
    public DateTime CreationTime { get; set; }
    public DateTime Deadline { get; set; }
    public string Status { get; set; }
    //public ICollection<Tag> Tags { get; }

    public List<ulong> Components { get; } = new();
}