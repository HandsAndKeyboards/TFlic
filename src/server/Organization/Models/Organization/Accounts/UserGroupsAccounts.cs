using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Accounts;

[Table("user_groups_accounts")]
public class UserGroupsAccounts
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public ulong Id { get; set; }
    
    [Column("user_group_id")]
    public ulong UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
    
    [Column("account_id")]
    public ulong AccountId { get; set; }
    public Account Account { get; set; } = null!;
}