using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Authentication;

/// <summary>
/// Класс содержит набор групп пользователей, участником которых является заданный аккаунт.
/// </summary>
/// <remarks>
/// класс содержит словарь { OrganizationId : userGroups }, где userGroups - число, представляющее результат
/// побитового сложения локальных Id групп пользователей организации, в которых состоит польщователь
/// </remarks>
public class AccountsUserGroups
{
    #region Public

    public AccountsUserGroups() { }
    
    public AccountsUserGroups(Dictionary<ulong, short> userGroups)
    {
        UserGroups = userGroups;
    }

    public AccountsUserGroups(Account account)
    {
        using var userGroupContext = DbContexts.Get<UserGroupContext>();
        using var orgContext = DbContexts.Get<OrganizationContext>();
        
        var organizations = account.GetOrganizations();
        
        foreach (var organization in organizations)
        {
            var userGroups = organization
                .GetUserGroups()
                .Where(ug => ug.Contains(account.Id) is not null);

            UserGroups.Add(organization.Id, CompactUserGroups(userGroups));
        }
    }

    public static ICollection<short> Decode(short encodedUserGroups)
    {
        var userGroups = new List<short>();
        
        for (short i = 0; i < sizeof(short) * 8; ++i)
        {
            if ((encodedUserGroups & (1 << i)) != 0)
            {
                userGroups.Add((short) (1 << i));
            }
        }
        
        return userGroups;
    }

    public bool ContainsOrganization(ulong organizationId) =>
        UserGroups.ContainsKey(organizationId);

    public Dictionary<ulong, short> UserGroups { get; set; } = new();
    #endregion



    #region Private
    private static short CompactUserGroups(IEnumerable<UserGroup> userGroups)
    {
        short compacted = 0;
        foreach (var userGroup in userGroups) { compacted |= userGroup.LocalId; }

        return compacted;
    }
    #endregion
}

// /// <summary>
// /// Класс содержит набор групп пользователей, участником которых является заданный аккаунт.
// /// </summary>
// /// <remarks>
// /// класс содержит множество словарей { OrganizationId : [ UserGroupLocalId_1, UserGroupLocalId_2 ] };
// /// </remarks>
// public record AccountsUserGroups
// {
//     public AccountsUserGroups(Account account)
//     {
//         using var userGroupContext = DbContexts.GetNotNull<UserGroupContext>();
//         using var orgContext = DbContexts.GetNotNull<OrganizationContext>();
//         
//         var organizations = account.GetOrganizations();
//         
//         foreach (var organization in organizations)
//         {
//             var userGroupIds = organization
//                 .GetUserGroups()
//                 .Where(ug => ug.Contains(account.Id) is not null)
//                 .Select(ug => ug.LocalId);
//             
//             var orgUserGroups = new Dictionary<ulong, IEnumerable<short>> {{organization.Id, userGroupIds}};
//             UserGroups.Add(orgUserGroups);
//         }
//     }
//
//     public List<Dictionary<ulong, IEnumerable<short>>> UserGroups { get; set; } = new();
// }