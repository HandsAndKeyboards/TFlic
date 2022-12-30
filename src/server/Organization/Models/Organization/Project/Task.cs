using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project.Component;

namespace Organization.Models.Organization.Project;

[Table("tasks")]
public class Task
{
    /// <summary>
    /// Уникальный идентификатор задачи
    /// </summary>
    [Required, Key, Column("id")]
    public ulong Id { get; init; }
    [Required, Column("column_id")]
    [ForeignKey("Column")]
    public ulong ColumnId { get; init; }
    
    public Column Column { get; set; }

    /// <summary>
    /// Позиция задачи в столбце
    /// </summary>
    [Required, Column("position")]
    public int Position { get; set; }
    /// <summary>
    /// Название задачи
    /// </summary>
    [Required, Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание задачи
    /// </summary>
    [Required, Column("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Авторы задачи
    /// </summary>
    [NotMapped]
    public ICollection<Account> Authors
    {
        get => _authors; init => _authors = (List<Account>)value;
    }
    private readonly List<Account> _authors = new();
    /// <summary>
    /// Время создания задачи
    /// </summary>
    [Required, Column("creation_time")]
    public DateTime CreationTime { get; set; }
    /// <summary>
    /// Текущий статус задачи
    /// </summary>
    [Required, Column("status")]
    public string Status { get; set; } = string.Empty;

    public uint priority { get; set; } = 1;
    
    [Column("id_executor")]
    public ulong ExecutorId { get; init; }

    [Column("estimated_time")]
    public uint EstimatedTime { get; set; } = 1;
    
    [Column("deadline")]
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Компоненты задачи
    /// </summary>
    public ICollection<ComponentDto> Components { get; set; }
}