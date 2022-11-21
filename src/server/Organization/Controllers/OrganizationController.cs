using System.Diagnostics.CodeAnalysis;
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

    [HttpPost("")]
    public IActionResult PostOrganizations()
    {
        // todo
        return ResponseGenerator.Ok("Еще не реализовано");
    }

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

    [HttpPatch("{OrganizationId:long}")]
    public IActionResult PatchOrganization(long OrganizationId)
    {
        // todo
        return ResponseGenerator.Ok("Еще не реализовано");
    }

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

    [HttpGet("{OrganizationId}/UserGroups")]
    public IActionResult GetOrganizationsUserGroups(ulong OrganizationId)
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
    
    [HttpPost("{OrganizationId:long}/UserGroups/{UserGroupId:long}/Members/{MemberId:long}")]
    public IActionResult PostMemberToOrganizationsUserGroup(long OrganizationId, long UserGroupId, long MemberId)
    {
        // todo
        return ResponseGenerator.Ok();
    }
    
    [HttpDelete("{OrganizationId}/UserGroups/{UserGroupLocalId}/Members/{MemberId}")]
    public IActionResult DeleteMemberFromOrganizationsUserGroup(ulong OrganizationId, short UserGroupLocalId, ulong MemberId)
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