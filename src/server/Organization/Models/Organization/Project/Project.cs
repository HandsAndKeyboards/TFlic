using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Organization.Project;
[Table("projects")]
public class Project : IUserAggregator
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public required ulong id { get; set; }

    [Column("organization_id")]
    public required ulong OrganizationId { get; set; }
    public required string name { get; set; }

    public required bool is_archived { get; set; }

    public ICollection<Board> boards { get; set; }

    [NotMapped]
    ICollection<Account> Members { get; init; }
    public bool AddAccount(Account account)
    {
        throw new NotImplementedException();
    }

    public Account? RemoveAccount(ulong id)
    {
        throw new NotImplementedException();
    }

    public Account? Contains(ulong id)
    {
        throw new NotImplementedException();
    }
}