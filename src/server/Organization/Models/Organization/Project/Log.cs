using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Organization.Project;

/*
    Пока отслеживает только изменение времени выполнения задачи
*/
[Table("logs")]
public class Log 
{
    /// <summary>
    /// Уникальный идентификатор лога
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong id { get; set; }

    [Column("organization_id")]
    public required ulong OrganizationId { get; set; }

    [Column("project_id")]
    public required ulong ProjectId { get; set; }

    /// <summary>
    /// Идентификатор задачи, изменения которой записываем
    /// </summary>
    [Column("task_id")]
    [ForeignKey("Task")]
    public required ulong TaskId { get; set; }

    /// <summary>
    /// Старое примерное время выполнения задачи, которое ставит юзер
    /// </summary>
    [Column("old_estimated_time")]
    public required ulong old_estimated_time { get; set; }

    /// <summary>
    /// Новое примерное время выполнения задачи, которое ставит юзер
    /// </summary>
    [Column("new_estimated_time")]
    public required ulong new_estimated_time { get; set; }

    /// <summary>
    /// Итоговое время выполнения задачи, которое ставит юзер
    /// </summary>
    [Column("real_time")]
    public required ulong real_time { get; set; }

    /// <summary>
    /// Спринт к которому относится задача
    /// </summary>
    [Column("sprint_number")]
    public required uint sprint_number { get; set; }

    [Column("edit_date")]
    public required DateTime edit_date { get; set; }

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