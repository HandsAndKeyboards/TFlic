using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Project;
[Table("projects")]
public class Project
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong id { get; set; }

    [Column("organization_id")]
    [ForeignKey("Organization")]
    public ulong OrganizationId { get; set; }

    public Organization Organization { get; set; }

    public string name { get; set; }

    public bool is_archived { get; set; }

    public ICollection<Board> boards { get; set; }
}