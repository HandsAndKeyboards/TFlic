namespace Organization.Models.Organization.Project;

public class Column
{
    /// <summary>
    /// Название столбца
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Допустимое количество задач в столбце
    /// </summary>
    public int LimitOfTask { get; set; }
    
    /// <summary>
    /// Задачи
    /// </summary>
    public ICollection<Task> Tasks { get; set; }
}