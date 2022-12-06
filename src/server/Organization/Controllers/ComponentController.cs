using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.POST;
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
    public IActionResult GetComponents(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        using var ComponentCtx = DbContexts.Get<ComponentContext>();
        var cmp = ComponentCtx.Components.Where(x => x.task_id == TaskId).
            Include(x => x.Task).
            ThenInclude(x => x.Column).
            ThenInclude(x => x.Board).
            ThenInclude(x => x.Project)
            .ToList();
        if (!cmp.All(x => x.Task.ColumnId == ColumnId &&
                     x.Task.Column.BoardId == BoardId &&
                     x.Task.Column.Board.ProjectId == ProjectId &&
                     x.Task.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        var cmps = cmp.Select(x => new DTO.GET.ComponentDto(x)).ToList();
        return Ok(cmps);
    }

    [HttpGet("Components/{ComponentId}")]
    public IActionResult GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        using var ComponentCtx = DbContexts.Get<ComponentContext>();
        var cmp = ComponentCtx.Components.Where(x => x.task_id == TaskId && x.id == ComponentId)
            .Include(x => x.Task)
            .ThenInclude(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!cmp.All(x => x.Task.ColumnId == ColumnId && x.Task.Column.BoardId == BoardId &&
                          x.Task.Column.Board.ProjectId == ProjectId &&
                          x.Task.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        var cmps = cmp.Select(x => new DTO.GET.ComponentDto(x)).ToList();
        return !cmps.Any() ? NotFound() : Ok(cmps.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Components/{ComponentId}")]
    public IActionResult DeleteComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        using var ComponentCtx = DbContexts.Get<ComponentContext>();
        var cmp = ComponentCtx.Components.Where(x => x.task_id == TaskId && x.id == ComponentId)
            .Include(x => x.Task)
            .ThenInclude(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!cmp.All(x => x.Task.ColumnId == ColumnId && x.Task.Column.BoardId == BoardId &&
                          x.Task.Column.Board.ProjectId == ProjectId &&
                          x.Task.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        ComponentCtx.Components.RemoveRange(ComponentCtx.Components.Where(x => x.id == ComponentId));
        ComponentCtx.SaveChanges();
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("Components")]
    public IActionResult PostComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ComponentDTO componentDto)
    {
        using var pathCtx = DbContexts.Get<TaskContext>();
        var prj = pathCtx.Tasks.Include(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Organization);
        if (!prj.Any(x => x.Id == TaskId && x.ColumnId == ColumnId && x.Column.BoardId == BoardId 
                          && x.Column.Board.ProjectId == ProjectId 
                          && x.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        
        using var ctx = DbContexts.Get<ComponentContext>();
        var obj = new ComponentDto()
        {
            name = componentDto.name, position = componentDto.position,
            component_type_id = componentDto.component_type_id, value = componentDto.value, task_id = TaskId
        };
        ctx.Components.Add(obj);
        ctx.SaveChanges();
        return Ok(obj);
    }
    #endregion
    
    #region PATCH
    [HttpPatch("Components/{ComponentId}")]
    public IActionResult PatchBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId,ulong TaskId,
        ulong ComponentId, [FromBody] JsonPatchDocument<ComponentDto> patch)
    {
        using var ctx = DbContexts.Get<ComponentContext>();
        
        var obj = ctx.Components
            .Include(x => x.Task)
            .ThenInclude(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .Single(x => x.id == ComponentId && x.task_id == TaskId
                    && x.Task.ColumnId == ColumnId && x.Task.Column.BoardId == BoardId 
                    && x.Task.Column.Board.ProjectId == ProjectId 
                    && x.Task.Column.Board.Project.OrganizationId == OrganizationId);
        
        patch.ApplyTo(obj);
        ctx.SaveChanges();
        
        return Ok(new DTO.GET.ComponentDto(obj));
    }
    #endregion
}