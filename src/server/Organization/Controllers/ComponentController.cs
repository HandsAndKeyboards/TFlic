using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return ResponseGenerator.NotFound();
        var cmps = cmp.Select(x => new DTO.ComponentDto(x)).ToList();
        return ResponseGenerator.Ok(value: cmps);
    }

    [HttpGet("Components/{ComponentId}")]
    public IActionResult GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        using var ComponentCtx = DbContexts.GetNotNull<ComponentContext>();
        var cmp = ComponentCtx.Components.Where(x => x.task_id == TaskId && x.id == ComponentId)
            .Include(x => x.Task)
            .ThenInclude(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!cmp.All(x => x.Task.ColumnId == ColumnId && x.Task.Column.BoardId == BoardId &&
                          x.Task.Column.Board.ProjectId == ProjectId &&
                          x.Task.Column.Board.Project.OrganizationId == OrganizationId))
            return ResponseGenerator.NotFound();
        var cmps = cmp.Select(x => new DTO.ComponentDto(x)).ToList();
        return !cmps.Any() ? ResponseGenerator.NotFound() : ResponseGenerator.Ok(value: cmps.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Components/{ComponentId}")]
    public IActionResult DeleteComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        using var ComponentCtx = DbContexts.GetNotNull<ComponentContext>();
        var cmp = ComponentCtx.Components.Where(x => x.task_id == TaskId && x.id == ComponentId)
            .Include(x => x.Task)
            .ThenInclude(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!cmp.All(x => x.Task.ColumnId == ColumnId && x.Task.Column.BoardId == BoardId &&
                          x.Task.Column.Board.ProjectId == ProjectId &&
                          x.Task.Column.Board.Project.OrganizationId == OrganizationId))
            return ResponseGenerator.NotFound();
        ComponentCtx.Components.RemoveRange(ComponentCtx.Components.Where(x => x.id == ComponentId));
        ComponentCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }
    #endregion

    #region PUT
    [HttpPut("Components")]
    public IActionResult PutComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ComponentDto componentDto)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        using var ColCtx = DbContexts.Get<ColumnContext>();
        using var TaskCtx = DbContexts.Get<TaskContext>();
        using var ComponentCtx = DbContexts.Get<ComponentContext>();
        if(BoardCtx is null || ProjectCtx is null || ColCtx is null || TaskCtx is null || ComponentCtx is null)
            return ResponseGenerator.NotFound();
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        if (!BoardCtx.Boards.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        if (!ColCtx.Columns.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        if (!TaskCtx.Tasks.Any(x => x.ColumnId == ColumnId && x.Id == TaskId))
            return ResponseGenerator.NotFound();
        if (componentDto.task_id != TaskId)
            return ResponseGenerator.NotFound();
        ComponentCtx.Components.Add(componentDto);
        ComponentCtx.SaveChanges();
        return ResponseGenerator.Ok(value: ComponentCtx.Components
            .Where(x => x.task_id == TaskId).ToList());
    }
    #endregion
}