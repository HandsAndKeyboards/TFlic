using System.ComponentModel.DataAnnotations;
using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project.Component;

namespace Organization.Models.Organization.Project;

public class Task
{
    /// <summary>
    /// Уникальный идентификатор задачи
    /// </summary>
    [Required]
    public long Id { get; init; }
    /// <summary>
    /// Позиция задачи в столбце
    /// </summary>
    [Required]
    public int Position { get; set; }
    /// <summary>
    /// Название задачи
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание задачи
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Связанные задачи
    /// </summary>
    public ICollection<Task> AssociatedTasks
    {
        get => _associatedTasks;
        init => _associatedTasks = (List<Task>)value;
    }

    private readonly List<Task> _associatedTasks = new();

    /// <summary>
    /// Авторы задачи
    /// </summary>
    public ICollection<IAccount> Authors
    {
        get => _authors; init => _authors = (List<IAccount>)value;
    }
    private readonly List<IAccount> _authors = new();
    /// <summary>
    /// Время создания задачи
    /// </summary>
    public DateTime CreationTime { get; set; }
    /// <summary>
    /// Текущий статус задачи
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Теги задачи 
    /// </summary>
    public ICollection<Tag> Tags
    {
        get => _tags; init => _tags = (List<Tag>)value;
    }
    private readonly List<Tag> _tags = new();
    
    /// <summary>
    /// Компоненты задачи
    /// </summary>
    public ICollection<IComponent> Components 
    {
        get => _components; init => _components = (List<IComponent>)value;
    }
    private readonly List<IComponent> _components = new();
}