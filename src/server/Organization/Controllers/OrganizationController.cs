using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Controllers;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[ApiController]
[Route("Organizations")]
public class OrganizationController
{
    #region Public
    public OrganizationController(ILogger<OrganizationController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Получение сведений об организации с указанным Id
    /// </summary>
    [HttpGet("{OrganizationId}")]
    public IActionResult GetOrganization(ulong OrganizationId)
    {
        var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var organizations = orgContext.Organizations
            .Where(org => org.Id == OrganizationId)
            .ToList();
        
        if (organizations.Count < 1) { Handlers.HandleElementNotFound(nameof(Organization), OrganizationId); }
        if (organizations.Count > 1) { Handlers.HandleFoundMultipleElementsWithSameId(nameof(Organization), OrganizationId); }

        return ResponseGenerator.Ok(value: new DTO.Organization(organizations[0]));
    }

    /// <summary>
    /// Регистрация организации в системе
    /// </summary>
    [HttpPost("")]
    public IActionResult RegisterOrganization([FromBody] DTO.RegistrationOrganization dtoOrganization)
    {
        // todo продумать добавление создателя организвции как ее админа
        
        var newOrganization = new Models.Organization.Organization
        {
            Name = dtoOrganization.Name,
            Description = dtoOrganization.Description
        };
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        orgContext.Add(newOrganization);
        orgContext.SaveChanges();
        /*
         * добавлять группы пользователей можно только после сохранения организации в бд,
         * так как до сохранения организация не имеет Id (его выдает база данных), вследствие
         * чего группам пользователей выдается некорректный LocalId 
         */
        newOrganization.CreateUserGroups();

        return ResponseGenerator.Ok(value: new DTO.Organization(newOrganization));
    }

    /// <summary>
    /// Редактирование организации
    /// </summary>
    [HttpPatch("{OrganizationId}")]
    public IActionResult EditOrganization(ulong OrganizationId, [FromBody] JsonPatchDocument<Models.Organization.Organization> patch)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == OrganizationId);
        if (organization is null) { return ResponseGenerator.NotFound($"Cannot find organization with Id = {OrganizationId}"); }
        
        patch.ApplyTo(organization);
        orgContext.SaveChanges();
        
        return ResponseGenerator.Ok(value: new DTO.Organization(organization));
    }

    /// <summary>
    /// Получение списка участников организации
    /// </summary>
    [HttpGet("{OrganizationId}/Members")]
    public IActionResult GetOrganizationsMembers(ulong OrganizationId)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == OrganizationId);
        if (organization is null) { return ResponseGenerator.NotFound($"Cannot find organization with Id = {OrganizationId}"); }

        var members = new List<Account>();
        foreach (var userGroup in organization.UserGroups) { members.AddRange(userGroup.Accounts); }

        var dtoMembers = members.Select(member => new DTO.Account(member)).ToList();
        return ResponseGenerator.Ok(value: dtoMembers);
    }

    /// <summary>
    /// Добавление пользователя в организацию
    /// </summary>
    [HttpPost("{OrganizationId}/Members")]
    public IActionResult AddUserToOrganization(ulong OrganizationId, [FromBody] ulong AccountId)
    {
        var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }

        Account account;
        try { account = accountContext.Accounts.Single(acc => acc.Id == AccountId); }
        catch (ArgumentNullException) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        catch (InvalidOperationException) { return Handlers.HandleFoundMultipleElementsWithSameId(nameof(Account), AccountId); }
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        Models.Organization.Organization organization;
        try { organization = orgContext.Organizations.Single(org => org.Id == OrganizationId); }
        catch (ArgumentNullException) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        catch (InvalidOperationException) { return Handlers.HandleFoundMultipleElementsWithSameId(nameof(Account), AccountId); }
        
        organization.AddAccount(account);

        return ResponseGenerator.Ok();
    }

    /// <summary>
    /// Удаление пользователя из организации
    /// </summary>
    [HttpDelete("{OrganizationId}/Members/{MemberId}")]
    public IActionResult DeleteOrganizationsMember(ulong OrganizationId, ulong MemberId)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == OrganizationId);
        if (organization is null) { return Handlers.HandleElementNotFound(nameof(Models.Organization.Organization), OrganizationId);}

        var removed = organization.RemoveAccount(MemberId);
        
        return removed is not null
            ? ResponseGenerator.Ok()
            : Handlers.HandleElementNotFound(nameof(Account), MemberId);
    }

    /// <summary>
    /// Получение групп пользователей организации
    /// </summary>
    [HttpGet("{OrganizationId}/UserGroups")]
    public IActionResult GetUserGroups(ulong OrganizationId)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == OrganizationId);
        if (organization is null) { return Handlers.HandleElementNotFound(nameof(Models.Organization.Organization), OrganizationId);}

        var dtoUserGroups = organization.UserGroups
            .AsParallel()
            .Select(ug => new DTO.UserGroup(ug));
        
        return ResponseGenerator.Ok(value: dtoUserGroups);
    }

    /// <summary>
    /// Получение участников указанной группы пользователей в организации
    /// </summary>
    [HttpGet("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members")]
    public IActionResult GetUserGroupMembers(ulong OrganizationId, short UserGroupLocalId)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == OrganizationId);
        if (organization is null) { return Handlers.HandleElementNotFound(nameof(Models.Organization.Organization), OrganizationId);}

        var userGroup = organization.UserGroups.FirstOrDefault(ug => ug.LocalId == UserGroupLocalId);
        if (userGroup is null) { return Handlers.HandleElementNotFound("user group", UserGroupLocalId); }

        var accounts = userGroup.Accounts.Select(acc => new DTO.Account(acc));
        return ResponseGenerator.Ok(value: accounts);
    }
    
    /// <summary>
    /// Добавление аккаунта в указанную группу пользователей организации
    /// </summary>
    [HttpPost("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public IActionResult AddMemberToUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        Models.Organization.Organization? organization = null;
        try { organization = orgContext.Organizations.Single(org => org.Id == OrganizationId); }
        catch (ArgumentNullException) { return Handlers.HandleElementNotFound(nameof(Models.Organization.Organization), OrganizationId); }
        catch (InvalidOperationException) { return Handlers.HandleFoundMultipleElementsWithSameId(nameof(Models.Organization.Organization), OrganizationId); }

        if (organization.Contains(MemberId) is null)
        {
            return ResponseGenerator.BadRequestResult(
                $"User with Id = {MemberId} is not in organization with Id = {OrganizationId}. " +
                "The user must be added to organization before being added to any of its user groups"
            );
        }
        
        try { organization.AddAccountToGroup(MemberId, UserGroupLocalId); }
        catch (Organization.Models.Organization.OrganizationException) { return ResponseGenerator.BadRequestResult($"Organization with Id = {OrganizationId} doesnt contain a user group with local Id = {UserGroupLocalId}"); }
        
        return ResponseGenerator.Ok();
    }
    
    /// <summary>
    /// Удаление аккаунта из указанной группы пользователей организации
    /// </summary>
    [HttpDelete("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public IActionResult DeleteMemberFromUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == OrganizationId);
        if (organization is null) { return Handlers.HandleElementNotFound(nameof(Models.Organization.Organization), OrganizationId);}

        organization.RemoveAccountFromGroup(MemberId, UserGroupLocalId);

        return ResponseGenerator.Ok();
    }
    #endregion



    #region Private
    private readonly ILogger<OrganizationController> _logger;
    #endregion
}