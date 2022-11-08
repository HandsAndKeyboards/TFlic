namespace Organization.Models.Organization.Project;

public class Column
{
    /// <summary>
    /// Уникальный идентефикатор столбца
    /// </summary>
    public required long Id { get; init; }
    /// <summary>
    /// Название столбца
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Позиция столбца на доске
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Допустимое количество задач в столбце
    /// </summary>
    public int LimitOfTask { get; set; }

    /// <summary>
    /// Задачи
    /// </summary>
    public ICollection<Task> Tasks
    {
        get => _tasks;
        init => _tasks = (List<Task>)value;
    }
    private readonly List<Task> _tasks = new();

    public bool MoveTask(long id, int position)
    {
        if (Tasks.Count <= position || !ContainTask(id))
            return false;
        var targetTask = GetTask(id);
        targetTask.Position = position;
        foreach (var item in Tasks.Where(task => task.Position >= targetTask.Position))
            item.Position--;
        return true;
    }
    
    public bool ContainTask(long id)
    {
        return Tasks.Any(task => task.Id == id);
    }
    
    public Task GetTask(long id)
    {
        return Tasks.Single(task => task.Id == id);
    }

    public bool AddTask(Task targetTask)
    {
        targetTask.Position = Tasks.Max(task => task.Position) + 1;
        Tasks.Add(targetTask);
        return false;
    }
}