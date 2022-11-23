using System.Diagnostics.CodeAnalysis;
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
            return ResponseGenerator.NotFound();
        var cmps = cmp.Select(x => new DTO.Task(x)).ToList();
        return ResponseGenerator.Ok(value: cmps);
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
            return ResponseGenerator.NotFound();
        var tasksDto = tasks.Select(x => new DTO.Task(x)).ToList();
        if (!tasksDto.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: tasksDto);
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
            return ResponseGenerator.NotFound();
        if (!tasks.Any())
            return ResponseGenerator.NotFound();
        TaskCtx.RemoveRange(TaskCtx.Tasks.Where(x => x.Id == TaskId)
            .Include(x=> x.Components));
        TaskCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }
    
    #endregion

    #region PUT
    [HttpPut("Tasks")]
    public IActionResult PutTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, Task task)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        using var ColCtx = DbContexts.Get<ColumnContext>();
        using var TaskCtx = DbContexts.Get<TaskContext>();
        if(BoardCtx is null || ProjectCtx is null || ColCtx is null || TaskCtx is null)
            return ResponseGenerator.NotFound();
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        if (!BoardCtx.Boards.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        if (!ColCtx.Columns.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        if (task.ColumnId != ColumnId)
            return ResponseGenerator.NotFound();
        TaskCtx.Tasks.Add(task);
        TaskCtx.SaveChanges();
        return ResponseGenerator.Ok(value: TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId).ToList());
    }
    #endregion
}