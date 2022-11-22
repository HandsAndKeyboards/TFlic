using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Organization.Models.Organization.Project;

public class Tag
{
    /// <summary>
    /// Уникальный идентификатор тега
    /// </summary>
    [Required]
    public ulong Id { get; init; }
    /// <summary>
    /// Цвет тега
    /// </summary>
    public int Color { get; set; }
    /// <summary>
    /// Название тега
    /// </summary>
    public string Name { get; set; }
}