using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.GET;
using Organization.Controllers.DTO.POST;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project.Component;

namespace Organization.Controllers;

[ApiController]
[Route("Organizations/{OrganizationId}/Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ComponentController : ControllerBase
{
    private readonly ILogger<ComponentController> _logger;

    public ComponentController(ILogger<ComponentController> logger)
    {
        _logger = logger;
    }

    #region GET
    [HttpGet("Components")]
    public ActionResult<ICollection<ComponentGET>> GetComponents(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        if (!PathChecker.IsComponentPathCorrect(OrganizationId, ProjectId, BoardId, ColumnId, TaskId))
            return NotFound();
        using var ctx = DbContexts.Get<ComponentContext>();
        var cmp = ContextIncluder.GetComponent(ctx)
            .Where(x => x.task_id == TaskId)
            .Select(x => new ComponentGET(x))
            .ToList();
        return !cmp.Any() ? NotFound() : Ok(cmp);
    }

    [HttpGet("Components/{ComponentId}")]
    public ActionResult<ComponentGET> GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        if (!PathChecker.IsComponentPathCorrect(OrganizationId, ProjectId, BoardId, ColumnId, TaskId))
            return NotFound();
        using var ctx = DbContexts.Get<ComponentContext>();
        var cmp = ContextIncluder.GetComponent(ctx)
            .Where(x => x.task_id == TaskId && x.id == ComponentId)
            .Select(x => new ComponentGET(x))
            .ToList();
        return !cmp.Any() ? NotFound() : Ok(cmp.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Components/{ComponentId}")]
    public ActionResult DeleteComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        if (!PathChecker.IsComponentPathCorrect(OrganizationId, ProjectId, BoardId, ColumnId, TaskId))
            return NotFound();
        using var ComponentCtx = DbContexts.Get<ComponentContext>();
        var cmp = ContextIncluder.GetComponent(ComponentCtx).Where(x => x.id == ComponentId);
        ComponentCtx.Components.RemoveRange(cmp);
        ComponentCtx.SaveChanges();
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("Components")]
    public ActionResult PostComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ComponentDTO componentDto)
    {
        if (!PathChecker.IsComponentPathCorrect(OrganizationId, ProjectId, BoardId, ColumnId, TaskId))
            return NotFound();
        
        using var ctx = DbContexts.Get<ComponentContext>();
        var obj = new ComponentDto()
        {
            name = componentDto.name,
            position = componentDto.position,
            component_type_id = componentDto.component_type_id,
            value = componentDto.value,
            task_id = TaskId
        };
        ctx.Components.Add(obj);
        ctx.SaveChanges();
        return Ok(obj);
    }
    #endregion
    
    #region PATCH
    [HttpPatch("Components/{ComponentId}")]
    public ActionResult<ComponentGET> PatchComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId,ulong TaskId,
        ulong ComponentId, [FromBody] JsonPatchDocument<ComponentDto> patch)
    {
        if (!PathChecker.IsComponentPathCorrect(OrganizationId, ProjectId, BoardId, ColumnId, TaskId))
            return NotFound();
        using var ctx = DbContexts.Get<ComponentContext>();
        var obj = ContextIncluder.GetComponent(ctx).Where(x => x.id == ComponentId).ToList();
        patch.ApplyTo(obj.Single());
        ctx.SaveChanges();
        
        return Ok(new ComponentGET(obj.Single()));
    }
    #endregion
}