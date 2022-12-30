using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Project;

[Table("columns")]
public class Column
{
    /// <summary>
    /// Уникальный идентификатор столбца
    /// </summary>
    [Column("id")]
    public ulong Id { get; set; }
    
    [Column("board_id")]
    [ForeignKey("Board")]

    public ulong BoardId { get; set; }

    public Board Board { get; set; }

    /// <summary>
    /// Название столбца
    /// </summary>
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Позиция столбца на доске
    /// </summary>
    [Column("position")]
    public int Position { get; set; }

    /// <summary>
    /// Допустимое количество задач в столбце
    /// </summary>
    [NotMapped]
    //[Column("limit_of_task")]
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

    /// <summary>
    /// Переместить задачу в другую позицию
    /// </summary>
    /// <param name="id">id задачи</param>
    /// <param name="position">В какую позицию переложить задачу</param>
    /// <returns>Возвращает false, если не задачи с таким id нет или кол-во задач <= выбранной позиции</returns>
    public bool MoveTask(ulong id, int position)
    {
        if (Tasks.Count <= position || !ContainTask(id))
            return false;
        var targetTask = GetTask(id);
        targetTask.Position = position;
        foreach (var item in Tasks.Where(task => task.Position >= targetTask.Position))
            item.Position--;
        return true;
    }
    
    /// <summary>
    /// Проверка на наличие задачи с выбранным id
    /// </summary>
    /// <param name="id">id задачи</param>
    /// <returns>Возвращает true. если есть задача с выбранным id</returns>
    public bool ContainTask(ulong id)
    {
        return Tasks.Any(task => task.Id == id);
    }
    /// <summary>
    /// Возвращает задачу по id
    /// </summary>
    /// <param name="id">id задачи</param>
    /// <returns>Объект Task или null</returns>
    public Task? GetTask(ulong id)
    {
        return ContainTask(id) ? Tasks.Single(task => task.Id == id) : null;
    }

    /// <summary>
    /// Добавляет задачу в колонку 
    /// </summary>
    /// <param name="targetTask">Задача, которую желаем добавить в колонку</param>
    /// <returns>Возвращает false, если уже существует задача с таким id</returns>
    public bool AddTask(Task targetTask)
    {
        if(ContainTask(targetTask.Id))
            return false;
        targetTask.Position = Tasks.Max(task => task.Position) + 1;
        Tasks.Add(targetTask);
        return true;
    }
}