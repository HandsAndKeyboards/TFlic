using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.GET;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Prj = Organization.Models.Organization.Project;
using postDTO = Organization.Controllers.DTO.POST.ProjectDTO;

namespace Organization.Controllers;

#if AUTH
using Models.Authentication;
using Microsoft.AspNetCore.Authorization;
[Authorize]
#endif
[ApiController]
[Route("Organizations/{OrganizationId}")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    #region GET
    
    [HttpGet("Projects")]
    public ActionResult<ICollection<ProjectGET>> GetProjects(ulong OrganizationId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsProjectPathCorrect(OrganizationId))
                    return NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        var cmp = ContextIncluder.GetProject(ctx)
            .Where(x => x.OrganizationId == OrganizationId)
            .Select(x => new ProjectGET(x))
            .ToList();
        return !cmp.Any() ? NotFound() : Ok(cmp);
    }
    
    [HttpGet("Projects/{ProjectId}")]
    public ActionResult<ProjectGET> GetProject(ulong OrganizationId, ulong ProjectId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsProjectPathCorrect(OrganizationId))
            return NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        var cmp = ContextIncluder.GetProject(ctx)
            .Where(x => x.OrganizationId == OrganizationId && x.id == ProjectId)
            .Select(x => new ProjectGET(x))
            .ToList();
        return !cmp.Any() ? NotFound() : Ok(cmp.Single());
    }
    
    #endregion

    #region DELETE
    [HttpDelete("Projects/{ProjectId}")]
    public ActionResult DeleteProjects(ulong OrganizationId, ulong ProjectId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsProjectPathCorrect(OrganizationId))
            return NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        var cmp = ContextIncluder.GetProject(ctx).Where(x => x.id == ProjectId && x.OrganizationId == OrganizationId);
        ctx.Projects.RemoveRange(cmp);
        ctx.SaveChanges();
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("Projects")]
    public ActionResult PostProjects(ulong OrganizationId, postDTO project)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsProjectPathCorrect(OrganizationId))
            return NotFound();
        
        using var ctx = DbContexts.Get<ProjectContext>();
        var obj = new Prj.Project
        {
            name = project.Name,
            OrganizationId = OrganizationId
        };
        ctx.Projects.Add(obj);
        ctx.SaveChanges();
        return Ok(obj);
    }
    #endregion

    #region PATCH
    [HttpPatch("Projects/{ProjectId}")]
    public ActionResult<ProjectGET> PatchProject(ulong OrganizationId, ulong ProjectId, [FromBody] JsonPatchDocument<Prj.Project> patch)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsProjectPathCorrect(OrganizationId))
            return NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        var obj = ContextIncluder.GetProject(ctx)
            .Where(x => x.id == ProjectId && x.OrganizationId == OrganizationId)
            .ToList();
        patch.ApplyTo(obj.Single());
        ctx.SaveChanges();
        
        return Ok(new ProjectGET(obj.Single()));
    }
    #endregion
}
