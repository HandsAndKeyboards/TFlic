using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;
using Organization.Models.Organization.Project.Component;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers;

//TODO Сделать всякие защиты от дураков && сменить Projects и поменять PUT методы
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
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        var projects = ProjectCtx.Projects.Where(x => x.OrganizationId == OrganizationId)
            .Include(x => x.boards)
            .ToList();
        var projectsDto = projects.Select(x => new DTO.Project(x)).ToList();
        return ResponseGenerator.Ok(value: projectsDto);
    }
    
    [HttpGet("Projects/{ProjectId}")]
    public IActionResult GetProject(ulong OrganizationId, ulong ProjectId)
    {
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        var projects = ProjectCtx.Projects.
            Where(x => x.OrganizationId == OrganizationId && x.id == ProjectId)
            .Include(x => x.boards)
            .ToList();
        var projectsDto = projects.Select(x => new DTO.Project(x)).ToList();
        if (!projectsDto.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: projectsDto.Single());
    }

    [HttpDelete("Projects/{ProjectId}")]
    public IActionResult DeleteProjects(ulong OrganizationId, ulong ProjectId)
    {
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        var projects = ProjectCtx.Projects.
            Where(x => x.OrganizationId == OrganizationId && x.id == ProjectId).
            Include(x => x.boards)
            .ThenInclude(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components)
            .ToList();
        if (!projects.Any())
            return ResponseGenerator.NotFound();
        ProjectCtx.RemoveRange(projects);
        ProjectCtx.SaveChanges();
        return ResponseGenerator.Ok();
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
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var boards = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId)
            .Include(x => x.Project)
            .Include(x => x.Columns)
            .ToList();
        if (boards.Any(x => x.Project.OrganizationId != OrganizationId))
            return ResponseGenerator.NotFound();
        var boardsDto = boards.Select(x => new DTO.Board(x)).ToList();
        return ResponseGenerator.Ok(value: boardsDto);
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var boards = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Include(x => x.Project)
            .Include(x => x.Columns)
            .ToList();
        if (boards.Any(x => x.Project.OrganizationId != OrganizationId))
            return ResponseGenerator.NotFound();
        var boardsDto = boards.Select(x => new DTO.Board(x)).ToList();
        if (!boardsDto.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: boardsDto.Single());
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
    public IActionResult DeleteBoards(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var columns = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Include(x => x.Project).ToList();
        if (!columns.Any() && columns.Any(x => x.Project.OrganizationId != OrganizationId))
            return ResponseGenerator.NotFound();
        
        BoardCtx.RemoveRange(BoardCtx.Boards.Where(x => x.id == BoardId)
            .Include(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components));
        BoardCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }

    #endregion

    #region Columns

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns")]
    public IActionResult GetColumns(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var ColCtx = DbContexts.Get<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId)
            .Include(x => x.Tasks)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return ResponseGenerator.NotFound();
        var columnsDto = columns.Select(x => new DTO.Column(x)).ToList();
        return ResponseGenerator.Ok(value: columnsDto);
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var ColCtx = DbContexts.Get<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId)
            .Include(x => x.Tasks)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return ResponseGenerator.NotFound();
        var columnsDto = columns.Select(x => new DTO.Column(x)).ToList();
        if (!columnsDto.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: columnsDto.Single());
    }
    
    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult DeleteColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var ColCtx = DbContexts.Get<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!columns.Any())
            return ResponseGenerator.NotFound();
        
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return ResponseGenerator.NotFound();
        ColCtx.RemoveRange(ColCtx.Columns.Where(x => x.Id == ColumnId)
            .Include(x => x.Tasks)
            .ThenInclude(x => x.Components));
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
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult GetTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        using var TaskCtx = DbContexts.Get<TaskContext>();
        var tasks = TaskCtx.Tasks.Where(x => x.ColumnId == ColumnId)
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

    
    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
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

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        using var ComponentCtx = DbContexts.GetNotNull<ComponentContext>();
        var cmp = ComponentCtx.Components.Where(x => x.task_id == TaskId)
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

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
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
        if (componentDto.task_id != TaskId)
            return ResponseGenerator.NotFound();
        ComponentCtx.Components.Add(componentDto);
        ComponentCtx.SaveChanges();
        return ResponseGenerator.Ok(value: ComponentCtx.Components
            .Where(x => x.task_id == TaskId).ToList());
    }

    #endregion
}