using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;

namespace Organization.Controllers;

using ModelAccount = Models.Organization.Accounts.Account;
using ModelOrganizationException = Models.Organization.OrganizationException;
using ModelUserGroup = Models.Organization.Accounts.UserGroup;
using ModelOrganization = Models.Organization.Organization;

#if AUTH
using Models.Authentication;
using Microsoft.AspNetCore.Authorization;
[Authorize]
#endif
[SuppressMessage("ReSharper", "InconsistentNaming")]
[ApiController]
[Route("Organizations")]
public class OrganizationController : ControllerBase
{
    /// <summary>
    /// Получение сведений об организации с указанным Id
    /// </summary>
    [HttpGet("{OrganizationId}")]
    public IActionResult GetOrganization(ulong OrganizationId)
    {
        var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var (error, organization) = DbValueRetriever.RetrieveFromDb(
            orgContext.Organizations.Include(org => org.Projects), 
            nameof(ModelOrganization.Id), 
            OrganizationId
        );
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);

        return ResponseGenerator.Ok(value: new DTO.Organization(organization!));
    }

    /// <summary>
    /// Регистрация организации в системе
    /// </summary>
    [HttpPost]
    public IActionResult RegisterOrganization([FromBody] DTO.RegisterOrganizationRequest registrationRequest)
    {
        // todo продумать добавление создателя организвции как ее админа
        
        var newOrganization = new ModelOrganization
        {
            Name = registrationRequest.Name,
            Description = registrationRequest.Description
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
        newOrganization.AddAccount(registrationRequest.CreatorId);
        newOrganization.AddAccountToGroup(registrationRequest.CreatorId, (short) ModelOrganization.PrimaryUserGroups.Admins);

        return ResponseGenerator.Ok(value: new DTO.Organization(newOrganization));
    }

    /// <summary>
    /// Редактирование организации
    /// </summary>
    [HttpPatch("{OrganizationId}")]
    public IActionResult EditOrganization(ulong OrganizationId, [FromBody] JsonPatchDocument<ModelOrganization> patch)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var (error, organization) = DbValueRetriever.RetrieveFromDb(
            orgContext.Organizations.Include(org => org.Projects), 
            nameof(ModelOrganization.Id), 
            OrganizationId
        );
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);
        
        patch.ApplyTo(organization!);
        orgContext.SaveChanges();
        
        return ResponseGenerator.Ok(value: new DTO.Organization(organization!));
    }

    /// <summary>
    /// Получение списка участников организации
    /// </summary>
    [HttpGet("{OrganizationId}/Members")]
    public IActionResult GetOrganizationMembers(ulong OrganizationId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, allowNoRole: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var (error, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);
        
        var members = new List<ModelAccount>();
        foreach (var userGroup in organization!.GetUserGroups()) { members.AddRange(userGroup.Accounts); }

        var dtoMembers = members.Select(member => new DTO.Account(member)).ToList();
        return ResponseGenerator.Ok(value: dtoMembers);
    }

    /// <summary>
    /// Добавление пользователя в организацию
    /// </summary>
    [HttpPost("{OrganizationId}/Members")]
    public IActionResult AddUserToOrganization(ulong OrganizationId, [FromBody] ulong AccountId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        var (accError, account) = DbValueRetriever.RetrieveFromDb(accountContext.Accounts, nameof(ModelAccount.Id), AccountId);
        if (accError is not null)
            return accError.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelAccount), AccountId)
                : Handlers.HandleException(accError);
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        var (orgError, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (orgError is not null)
            return orgError.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(orgError);
        
        organization!.AddAccount(account!);

        return ResponseGenerator.Ok();
    }

    /// <summary>
    /// Удаление пользователя из организации
    /// </summary>
    [HttpDelete("{OrganizationId}/Members/{MemberId}")]
    public IActionResult DeleteOrganizationsMember(ulong OrganizationId, ulong MemberId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        var (error, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);
        
        var removed = organization!.RemoveAccount(MemberId);
        
        return removed is not null
            ? ResponseGenerator.Ok()
            : Handlers.HandleElementNotFound(nameof(ModelAccount), MemberId);
    }

    /// <summary>
    /// Получение групп пользователей организации
    /// </summary>
    [HttpGet("{OrganizationId}/UserGroups")]
    public IActionResult GetUserGroups(ulong OrganizationId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, allowNoRole: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var (error, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);

        var dtoUserGroups = organization!.GetUserGroups()
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
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, allowNoRole: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }
        
        var (orgError, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (orgError is not null)
            return orgError.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(orgError);
        
        var (groupError, userGroup) = DbValueRetriever.RetrieveFromDb(organization!.GetUserGroups(), nameof(ModelUserGroup.LocalId), UserGroupLocalId);
        if (groupError is not null)
            return groupError.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelUserGroup), UserGroupLocalId)
                : Handlers.HandleException(groupError);
        
        var accounts = userGroup!.Accounts.Select(acc => new DTO.Account(acc));
        return ResponseGenerator.Ok(value: accounts);
    }
    
    /// <summary>
    /// Добавление аккаунта в указанную группу пользователей организации
    /// </summary>
    [HttpPost("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public IActionResult AddMemberToUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var (error, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);

        if (organization!.Contains(MemberId) is null)
        {
            return ResponseGenerator.BadRequestResult(
                $"User with Id = {MemberId} is not in organization with Id = {OrganizationId}. " +
                "The user must be added to organization before being added to any of its user groups"
            );
        }

        try { organization.AddAccountToGroup(MemberId, UserGroupLocalId); }
        catch (ModelOrganizationException) { return ResponseGenerator.BadRequestResult($"Organization with Id = {OrganizationId} doesnt contain a user group with local Id = {UserGroupLocalId}"); }
        
        return ResponseGenerator.Ok();
    }
    
    /// <summary>
    /// Удаление аккаунта из указанной группы пользователей организации
    /// </summary>
    [HttpDelete("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public IActionResult DeleteMemberFromUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return ResponseGenerator.Forbidden(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        if (orgContext is null) { return Handlers.HandleNullDbContext(typeof(OrganizationContext)); }

        var (error, organization) = DbValueRetriever.RetrieveFromDb(orgContext.Organizations, nameof(ModelOrganization.Id), OrganizationId);
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? Handlers.HandleElementNotFound(nameof(ModelOrganization), OrganizationId)
                : Handlers.HandleException(error);

        try
        {
            organization!.RemoveAccountFromGroup(MemberId, UserGroupLocalId);
        }
        catch (Exception)
        {
            return Handlers.HandleException(
                $"Cannot remove account with Id = {MemberId} " +
                $"from usergroup (local Id = {UserGroupLocalId}) " +
                $"of organization with Id = {OrganizationId}"
            ); 
        }

        return ResponseGenerator.Ok();
    }
}