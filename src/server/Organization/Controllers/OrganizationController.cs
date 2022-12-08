using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO;
using Organization.Controllers.Service;
using Organization.Models.Authentication;
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
    public ActionResult<OrganizationDto> GetOrganization(ulong OrganizationId)
    {
        var orgContext = DbContexts.Get<OrganizationContext>();

        var organization = DbValueRetriever.Retrieve(
            orgContext.Organizations.Include(org => org.Projects), 
            OrganizationId, 
            nameof(ModelOrganization.Id)
        );

        return organization is not null
            ? Ok(new OrganizationDto(organization))
            : NotFound();
    }

    /// <summary>
    /// Регистрация организации в системе
    /// </summary>
    [HttpPost]
    public ActionResult<OrganizationDto> RegisterOrganization([FromBody] RegisterOrganizationRequestDto registrationRequest)
    {
        var accountContext = DbContexts.Get<AccountContext>();
        var creator = DbValueRetriever.Retrieve(accountContext.Accounts, registrationRequest.CreatorId, nameof(ModelAccount.Id));
        if (creator is null) { return BadRequest($"account with id = {registrationRequest.CreatorId} doesnt exist"); }
        
        var newOrganization = new ModelOrganization
        {
            Name = registrationRequest.Name,
            Description = registrationRequest.Description
        };
        
        using var orgContext = DbContexts.Get<OrganizationContext>();

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

        return Ok(new OrganizationDto(newOrganization));
    }

    /// <summary>
    /// Редактирование организации
    /// </summary>
    [HttpPatch("{OrganizationId}")]
    public ActionResult<OrganizationDto> EditOrganization(ulong OrganizationId, [FromBody] JsonPatchDocument<ModelOrganization> patch)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();

        var organization = DbValueRetriever.Retrieve(
            orgContext.Organizations.Include(org => org.Projects), 
            OrganizationId, 
            nameof(ModelOrganization.Id)
        );
        if (organization is null) { return NotFound(); }
        
        patch.ApplyTo(organization);
        orgContext.SaveChanges();
        
        return Ok(new OrganizationDto(organization));
    }

    /// <summary>
    /// Получение списка участников организации
    /// </summary>
    [HttpGet("{OrganizationId}/Members")]
    public ActionResult<IEnumerable<AccountDto>> GetOrganizationMembers(ulong OrganizationId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, allowNoRole: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();

        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }
        
        var members = new List<ModelAccount>();
        foreach (var userGroup in organization.GetUserGroups()) { members.AddRange(userGroup.Accounts); }

        var dtoMembers = members.Select(member => new AccountDto(member));
        return Ok(dtoMembers);
    }

    /// <summary>
    /// Добавление пользователя в организацию
    /// </summary>
    [HttpPost("{OrganizationId}/Members")]
    public ActionResult AddUserToOrganization(ulong OrganizationId, [FromBody] string login)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif

        using var authInfoContext = DbContexts.Get<AuthInfoContext>();
        var authInfo = DbValueRetriever.Retrieve(authInfoContext.Info, login, nameof(AuthInfo.Login));

        if (authInfo is null) { return NotFound(); }
        
        using var accountContext = DbContexts.Get<AccountContext>();
        var account = DbValueRetriever.Retrieve(accountContext.Accounts, authInfo.AccountId, nameof(ModelAccount.Id));
        if (account is null) { return NotFound(); }
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }

        organization.AddAccount(account);

        return Ok();
    }

    /// <summary>
    /// Удаление пользователя из организации
    /// </summary>
    [HttpDelete("{OrganizationId}/Members/{MemberId}")]
    public ActionResult DeleteOrganizationsMember(ulong OrganizationId, ulong MemberId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        
        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }
        
        var removed = organization.RemoveAccount(MemberId);
        
        return removed is not null
            ? Ok()
            : NotFound();
    }

    /// <summary>
    /// Получение групп пользователей организации
    /// </summary>
    [HttpGet("{OrganizationId}/UserGroups")]
    public ActionResult<IEnumerable<UserGroupDto>> GetUserGroups(ulong OrganizationId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, allowNoRole: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();

        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }
        
        var dtoUserGroups = organization.GetUserGroups().Select(ug => new UserGroupDto(ug));
        return Ok(dtoUserGroups);
    }

    /// <summary>
    /// Получение участников указанной группы пользователей в организации
    /// </summary>
    [HttpGet("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members")]
    public ActionResult<IEnumerable<AccountDto>> GetUserGroupMembers(ulong OrganizationId, short UserGroupLocalId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, allowNoRole: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();
        
        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }
        
        var userGroup = DbValueRetriever.Retrieve(organization.GetUserGroups(), UserGroupLocalId, nameof(ModelUserGroup.LocalId));
        if (userGroup is null) { return NotFound(); }

        var accounts = userGroup.Accounts.Select(acc => new AccountDto(acc));
        return Ok(accounts);
    }
    
    /// <summary>
    /// Добавление аккаунта в указанную группу пользователей организации
    /// </summary>
    [HttpPost("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public ActionResult AddMemberToUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();

        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }
        
        if (organization.Contains(MemberId) is null)
        {
            return BadRequest(
                $"User with Id = {MemberId} is not in organization with Id = {OrganizationId}. " +
                "The user must be added to organization before being added to any of its user groups"
            );
        }

        try { organization.AddAccountToGroup(MemberId, UserGroupLocalId); }
        catch (ModelOrganizationException) { return BadRequest($"Organization with Id = {OrganizationId} doesnt contain a user group with local Id = {UserGroupLocalId}"); }
        
        return Ok();
    }
    
    /// <summary>
    /// Удаление аккаунта из указанной группы пользователей организации
    /// </summary>
    [HttpDelete("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public ActionResult DeleteMemberFromUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        
        using var orgContext = DbContexts.Get<OrganizationContext>();

        var organization = DbValueRetriever.Retrieve(orgContext.Organizations, OrganizationId, nameof(ModelOrganization.Id));
        if (organization is null) { return NotFound(); }

        organization.RemoveAccountFromGroup(MemberId, UserGroupLocalId);
        return Ok();
    }
}