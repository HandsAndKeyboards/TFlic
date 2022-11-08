using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project.Component;

namespace Organization.Models.Organization.Project;

public class Task
{
    /// <summary>
    /// Уникальный идентефикатор задачи
    /// </summary>
    public required long Id { get; init; }
    /// <summary>
    /// Позиция задачи в столбце
    /// </summary>
    public required int Position { get; set; }
    /// <summary>
    /// Название задачи
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Описание задачи
    /// </summary>
    public required string Description { get; set; }

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
    public string Status { get; set; }
    /// <summary>
    /// Теги задачи 
    /// </summary>
    public ICollection<Tag> tags { get; set; }
    /// <summary>
    /// Компоненты задачи
    /// </summary>
    public ICollection<IComponent> Components { get; set; }
}