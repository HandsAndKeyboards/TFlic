using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;
using Organization.Models.Organization.Project.Component;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers;

//TODO Сделать всякие защиты от дураков
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

    #region Projects

    [HttpGet("Projects")]
    public IActionResult GetProjects(ulong OrganizationId)
    {
        using var ctx = DbContexts.Get<ProjectContext>();
        if (ctx == null)
            return ResponseGenerator.NotFound();
        var entities = ctx.Projects
            .Where(x => x.OrganizationId == OrganizationId)
            .Include(x => x.boards)
            .ThenInclude(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components);
        return ResponseGenerator.Ok(value: entities.ToList());
    }

    [HttpDelete("Projects/{ProjectId}")]
    public IActionResult DeleteProjects(ulong OrganizationId, ulong ProjectId)
    {
        using var ctx = DbContexts.Get<ProjectContext>();
        if (ctx == null)
            return ResponseGenerator.NotFound();
        var table = ctx.Projects;
        var entities = table.Where(x => x.id == ProjectId && x.OrganizationId == OrganizationId);
        table.RemoveRange(entities);
        ctx.SaveChanges();
        return ResponseGenerator.Ok(value: table.ToList());
    }

    [HttpPut("Projects")]
    public IActionResult PutProjects(ulong OrganizationId, Project project)
    {
        if (project.OrganizationId != OrganizationId)
            return ResponseGenerator.NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        if (ctx == null)
            return ResponseGenerator.NotFound();
        ctx.Projects.Add(project);
        ctx.SaveChanges();
        return ResponseGenerator.Ok(value: ctx.Projects.ToList());
    }

    #endregion

    #region Boards

    [HttpGet("Projects/{ProjectId}/Boards")]
    public IActionResult GetBoards(ulong OrganizationId, ulong ProjectId)
    {
        using var ctx = DbContexts.Get<BoardContext>();
        if (ctx == null)
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: ctx.Boards.
            Where(x => x.ProjectId == ProjectId).
            Include(x=> x.Columns).
            ThenInclude(x=> x.Tasks).
            ThenInclude(x =>x.Components).
            ToList());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var ctx = DbContexts.Get<BoardContext>();
        if(ctx == null)
            return ResponseGenerator.NotFound();

        var table = ctx.Boards;
        var entities = table.Where(x => x.id == BoardId).
            Include(x => x.Columns).
            ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components);
        return entities.Count() == 1 ? ResponseGenerator.Ok(value: entities.Single()) : ResponseGenerator.NotFound();
    }

    [HttpPut("Projects/{ProjectId}/Boards")]
    public IActionResult GetBoards(ulong OrganizationId, ulong ProjectId, Board board)
    {
        using var ctx = DbContexts.Get<BoardContext>();
        if(ctx == null)
            return ResponseGenerator.NotFound();   
        ctx.Boards.Add(board);
        ctx.SaveChanges();
        return ResponseGenerator.Ok(value: ctx.Boards.Where(x => x.ProjectId == ProjectId).ToList());
    }

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult DeleteBoards(ulong OrganizationId,
        ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        if(BoardCtx is null || ProjectCtx is null)
            return ResponseGenerator.NotFound();
        var table = BoardCtx.Boards;
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var entities = table.Where(x => x.id == BoardId && x.ProjectId == ProjectId);
        if (!entities.Any())
            return ResponseGenerator.NotFound();
        table.RemoveRange(entities);
        BoardCtx.SaveChanges();
        return ResponseGenerator.Ok(value: table.ToList());
    }

    #endregion

    #region Columns

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns")]
    public IActionResult GetColumns(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        using var ColCtx = DbContexts.Get<ColumnContext>();
        if(BoardCtx is null || ProjectCtx is null || ColCtx is null)
            return ResponseGenerator.NotFound();
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        if (!BoardCtx.Boards.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = ColCtx.Columns;
        return ResponseGenerator.Ok(value: col.Where(x => x.BoardId == BoardId).ToList());
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        using var ColCtx = DbContexts.Get<ColumnContext>();
        if(BoardCtx is null || ProjectCtx is null || ColCtx is null)
            return ResponseGenerator.NotFound();
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        if (!BoardCtx.Boards.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = ColCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId);
        return !col.Any() ? ResponseGenerator.NotFound() : ResponseGenerator.Ok(value: col.Single());
    }
    
    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult DeleteColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        using var ColCtx = DbContexts.Get<ColumnContext>();
        if(BoardCtx is null || ProjectCtx is null || ColCtx is null)
            return ResponseGenerator.NotFound();
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        if (!BoardCtx.Boards.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = ColCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId);
        ColCtx.RemoveRange(col);
        ColCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }
    
    [HttpPut("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult PutColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, Column Column)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        using var ColCtx = DbContexts.Get<ColumnContext>();
        if(BoardCtx is null || ProjectCtx is null || ColCtx is null)
            return ResponseGenerator.NotFound();
        if (!ProjectCtx.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        if (!BoardCtx.Boards.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        if(Column.BoardId != BoardId)
            return ResponseGenerator.NotFound();
        ColCtx.Add(Column);
        ColCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }

    #endregion

    #region Tasks

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks")]
    public IActionResult GetTasks(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
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

        return ResponseGenerator.Ok(value: TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId).ToList());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult GetTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
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
        var task = TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId && x.Id == TaskId);
        if (!task.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: task.Single());
    }

    
    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult DeleteTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
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
        var task = TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId && x.Id == TaskId);
        if (!task.Any())
            return ResponseGenerator.NotFound();
        TaskCtx.RemoveRange(task);
        return ResponseGenerator.Ok(value: TaskCtx.Tasks.ToList());
    }
    
    [HttpPut("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks")]
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

    #region Components

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components")]
    public IActionResult GetComponents(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
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
        return ResponseGenerator.Ok(value: ComponentCtx.Components.Where(x => x.TaskId == TaskId).ToList());
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
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
        var component = ComponentCtx.Components.Where(x => x.TaskId == TaskId && x.id == ComponentId);
        if (!component.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: component.Single());
    }

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult DeleteComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
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
        var component = ComponentCtx.Components.Where(x => x.TaskId == TaskId && x.id == ComponentId);
        if (!component.Any())
            return ResponseGenerator.NotFound();
        ComponentCtx.Components.RemoveRange(component);
        ComponentCtx.SaveChanges();
        return ResponseGenerator.Ok(value: ComponentCtx.Components.Where(x=> x.TaskId == TaskId).ToList());
    }
    
    [HttpPut("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components")]
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
        if (componentDto.TaskId != TaskId)
            return ResponseGenerator.NotFound();
        ComponentCtx.Components.Add(componentDto);
        ComponentCtx.SaveChanges();
        return ResponseGenerator.Ok(value: ComponentCtx.Components
            .Where(x => x.TaskId == TaskId).ToList());
    }

    #endregion
}