using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Organization.Models.Organization.Project
{
    [Table("sprints")]
    public class Sprint : ISprint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public ulong id { get; set; }

        [Column("organization_id")]
        public ulong OrganizationId { get; set; }

        [Column("project_id")]
        public ulong ProjectId { get; set; }

        [Column("begin_date")]
        public DateTime BeginDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("duration")]
        public ulong Duration { get; set; }

        [Column("number")]
        public uint Number { get; set; }
    }
}
