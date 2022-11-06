using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project.Component;

namespace Organization.Models.Organization.Project;

public class Task
{
    /// <summary>
    /// Название задачи
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Связанные задачи
    /// </summary>
    public ICollection<Task> associatedTasks { get; set; }
    /// <summary>
    /// Авторы задачи
    /// </summary>
    public ICollection<IAccount> Authors { get; set; }
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