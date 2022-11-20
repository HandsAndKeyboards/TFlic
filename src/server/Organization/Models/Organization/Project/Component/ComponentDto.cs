using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Organization.Models.Organization.Project.Component;

[Table("components")]
public class ComponentDto
{
    [Column("id")]
    public required ulong id { get; set; }
    [Column("task_id")]
    public required ulong TaskId { get; set; }

    [Column("component_type_id")]
    public required ulong component_type_id { get; set; }
    [Column("name")]
    public required string name { get; set; }
    [Column("value")]
    public required string value { get; set; }
}