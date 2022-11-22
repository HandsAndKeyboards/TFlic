using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Project.Component;

[Table("components")]
public class ComponentDto
{
    [Column("id")]
    public required ulong id { get; set; }
    [Column("task_id")]
    [ForeignKey("Task")]
    public required ulong task_id { get; set; }
    public Task Task { get; set; }

    [Column("component_type_id")]
    public required ulong component_type_id { get; set; }
    
    [Column("position")]
    public required int position { get; set; }
    [Column("name")]
    public required string name { get; set; }
    [Column("value")]
    public required string value { get; set; }
}