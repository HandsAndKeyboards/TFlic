using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Project;

[Table("boards")]
public class Board
{
    /// <summary>
    /// Уникальный идентификатор доски
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong id { get; set; }

    /// <summary>
    /// Название доски
    /// </summary>
    [Column("name")]
    public string Name { get; set; }

    [Column("project_id")]
    [ForeignKey("Project")]
    public ulong ProjectId { get; set; }
    public Project Project { get; set; }

    /// <summary>
    /// Столбцы доски
    /// </summary>
    //[NotMapped]
    public ICollection<Column> Columns
    {
        get => _columns;
        init => _columns = (List<Column>)value;
    }

    private readonly List<Column> _columns = new();
}