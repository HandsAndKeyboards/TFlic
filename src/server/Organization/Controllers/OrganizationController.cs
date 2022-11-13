using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Organization.Models.Organization;
using Organization.Models.Organization.Accounts;

using Organization.Controllers.Common;


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

    [HttpGet("")]
    public IEnumerable<IOrganization> GetOrganizations()
    {
        // todo
        return new List<IOrganization>();
    }

    [HttpPost("")]
    public IActionResult PostOrganizations()
    {
        // todo
        return ResponseGenerator.Ok();
    }

    [HttpPatch("Organizations/{OrganizationId:long}")]
    public IActionResult PostOrganization(long OrganizationId)
    {
        // todo
        return ResponseGenerator.Ok();
    }

    [HttpGet("Organizations/{OrganizationId:long}/Members")]
    public IActionResult GetOrganizationsMembers(long OrganizationId)
    {
        // todo
        return ResponseGenerator.Ok(value:new List<IAccount>());
    }

    [HttpDelete("Organizations/{OrganizationId:long}/Members/{MemberId:long}")]
    public IActionResult DeleteOrganizationsMember(long OrganizationId, long MemberId)
    {
        // todo
        return ResponseGenerator.Ok();
    }

    [HttpGet("Organizations/{OrganizationId:long}/UserGroups")]
    public IActionResult GetOrganizationsUserGroups(long OrganizationId)
    {
        // todo
        return ResponseGenerator.Ok(value:new List<IUserGroup>());
    }

    [HttpGet("Organizations/{OrganizationId:long}/UserGroups/{UserGroupId:long}/Members")]
    public IActionResult GetUserGroupMembers(long OrganizationId, long UserGroupId)
    {
        // todo
        return ResponseGenerator.Ok(value:new List<IUserGroup>());
    }
    
    [HttpPost("Organizations/{OrganizationId:long}/UserGroups/{UserGroupId:long}/Members/{MemberId:long}")]
    public IActionResult PostMemberToOrganizationsUserGroup(long OrganizationId, long UserGroupId, long MemberId)
    {
        // todo
        return ResponseGenerator.Ok();
    }
    
    [HttpDelete("Organizations/{OrganizationId:long}/UserGroups/{UserGroupId:long}/Members/{MemberId:long}")]
    public IActionResult DeleteMemberToOrganizationsUserGroup(long OrganizationId, long UserGroupId, long MemberId)
    {
        // todo
        return ResponseGenerator.Ok();
    }

    #endregion



    #region Private

    private readonly ILogger<OrganizationController> _logger;

    #endregion
}