using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers;

[ApiController]
[Route("Organizations/{OrganizationId}/Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;

    public TaskController(ILogger<TaskController> logger)
    {
        _logger = logger;
    }

    #region GET
    [HttpGet("Tasks")]
    public IActionResult GetTasks(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var TaskCtx = DbContexts.Get<TaskContext>();
        var cmp = TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId)
            .Include(x => x.Components)
            .Include(x => x.Column).
            ThenInclude(x => x.Board).
            ThenInclude(x => x.Project)
            .ToList();
        if (!cmp.All(x => x.Column.BoardId == BoardId && x.Column.Board.ProjectId == ProjectId &&
                          x.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        var cmps = cmp.Select(x => new DTO.GET.Task(x)).ToList();
        return Ok(cmps);
    }
    
    [HttpGet("Tasks/{TaskId}")]
    public IActionResult GetTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        using var TaskCtx = DbContexts.Get<TaskContext>();
        var tasks = TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId && x.Id == TaskId)
            .Include(x => x.Components)
            .Include(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!tasks.All(x => x.Column.BoardId == BoardId && x.Column.Board.ProjectId == ProjectId &&
                          x.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        var tasksDto = tasks.Select(x => new DTO.GET.Task(x)).ToList();
        if (!tasksDto.Any())
            return NotFound();
        return Ok(tasksDto);
    }
    #endregion

    #region DELETE
    [HttpDelete("Tasks/{TaskId}")]
    public IActionResult DeleteTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        using var TaskCtx = DbContexts.Get<TaskContext>();
        var tasks = TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId && x.Id == TaskId).
            Include(x => x.Column).
            ThenInclude(x => x.Board).
            ThenInclude(x => x.Project)
            .ToList();
        if (!tasks.All(x => x.Column.BoardId == BoardId && x.Column.Board.ProjectId == ProjectId &&
                            x.Column.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        if (!tasks.Any())
            return NotFound();
        TaskCtx.RemoveRange(TaskCtx.Tasks.Where(x => x.Id == TaskId)
            .Include(x=> x.Components));
        TaskCtx.SaveChanges();
        return Ok();
    }
    
    #endregion

    #region PATCH
    [HttpPatch("Tasks/{TaskId}")]
    public IActionResult PatchTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        [FromBody] JsonPatchDocument<Task> patch)
    {
        using var ctx = DbContexts.Get<TaskContext>();

        var obj = ctx.Tasks
            .Include(x => x.Components)
            .Include(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .Single(x => x.Id == TaskId && x.ColumnId == ColumnId && x.Column.BoardId == BoardId 
                && x.Column.Board.ProjectId == ProjectId && x.Column.Board.Project.OrganizationId == OrganizationId);
        
        patch.ApplyTo(obj);
        ctx.SaveChanges();
        
        return Ok(new DTO.GET.Task(obj));
    }
    #endregion
}