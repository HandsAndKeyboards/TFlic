namespace Organization.Controllers.DTO.GET;

public class TaskGET
{
    public TaskGET(Models.Organization.Project.Task task)
    {
        Id = task.Id;
        Position = task.Position;
        Name = task.Name;
        CreationTime = task.CreationTime;
        Description = task.Description;
        Status = task.Status;
        
        foreach (var component in task.Components) { Components.Add(component.id); }
    }
    
    public ulong Id { get; init; }
    
    public int Position { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }
    public List<ulong> Authors {get;} = new();
    public DateTime CreationTime { get; set; }
    public string Status { get; set; }
    //public ICollection<Tag> Tags { get; }

    public List<ulong> Components { get; } = new();
}