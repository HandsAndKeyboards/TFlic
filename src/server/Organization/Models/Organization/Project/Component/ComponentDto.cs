using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Project.Component;

[Table("components")]
public class ComponentDto
{
    [Column("id")]
    public ulong id { get; set; }
    [Column("task_id")]
    [ForeignKey("Task")]
    public ulong task_id { get; set; }
    public Task Task { get; set; }

    [Column("component_type_id")]
    public ulong component_type_id { get; set; }
    
    [Column("position")]
    public int position { get; set; }
    [Column("name")]
    public string name { get; set; }
    [Column("value")]
    public string value { get; set; }
}