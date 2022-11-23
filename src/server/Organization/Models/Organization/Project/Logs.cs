using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Organization.Project;

/*
    Пока отслеживает только изменение времени выполнения задачи
*/
[Table("logs")]
public class Logs 
{
    /// <summary>
    /// Уникальный идентификатор лога
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong id { get; set; }

    /// <summary>
    /// Идентификатор задачи, изменения которой записываем
    /// </summary>
    [Column("task_id")]
    [ForeignKey("Task")]
    public required ulong TaskId { get; set; }

    /// <summary>
    /// Примерное время выполнения задачи, которое ставит юзер
    /// </summary>
    [Column("current_estimated_time")]
    public required DateTime estimated_time { get; set; }

    /// <summary>
    /// Итоговое время выполнения задачи, которое ставит юзер
    /// </summary>
    [Column("current_real_time")]
    public required DateTime real_time { get; set; }

    /// <summary>
    /// Спринт к которому относится задача
    /// </summary>
    [Column("current_real_time")]
    public required uint sprint_number { get; set; }

    // [NotMapped]
    public Sprint SetAssociatedSprint(ulong id)
    {
        // - Посмотреть на дату изменения и поставить спринт который относится к этим датам
        throw new NotImplementedException();
    }

    public Task? ContainsTask(ulong id)
    {
        throw new NotImplementedException();
    }
}