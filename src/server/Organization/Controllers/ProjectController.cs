using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Common;
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
        var entities = DbContexts.ProjectContext.Projects
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
        var table = DbContexts.ProjectContext.Projects;
        var entities = table.Where(x => x.id == ProjectId && x.OrganizationId == OrganizationId);
        table.RemoveRange(entities);
        DbContexts.ProjectContext.SaveChanges();
        return ResponseGenerator.Ok(value: table.ToList());
    }

    [HttpPut("Projects")]
    public IActionResult PutProjects(ulong OrganizationId, Project project)
    {
        if (project.OrganizationId != OrganizationId)
            return ResponseGenerator.NotFound();
        DbContexts.ProjectContext.Projects.Add(project);
        DbContexts.ProjectContext.SaveChanges();
        return ResponseGenerator.Ok(value: DbContexts.ProjectContext.Projects.ToList());
    }

    #endregion

    #region Boards

    [HttpGet("Projects/{ProjectId}/Boards")]
    public IActionResult GetBoards(ulong OrganizationId, ulong ProjectId)
    {
        return ResponseGenerator.Ok(value: DbContexts.BoardContext.Boards.
            Where(x => x.ProjectId == ProjectId).
            Include(x=> x.Columns).
            ThenInclude(x=> x.Tasks).
            ThenInclude(x =>x.Components).
            ToList());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        var table = DbContexts.BoardContext.Boards;
        var entities = table.Where(x => x.id == BoardId).
            Include(x => x.Columns).
            ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components);
        return entities.Count() == 1 ? ResponseGenerator.Ok(value: entities.Single()) : ResponseGenerator.NotFound();
    }

    [HttpPut("Projects/{ProjectId}/Boards")]
    public IActionResult GetBoards(ulong OrganizationId, Board board)
    {
        DbContexts.BoardContext.Boards.Add(board);
        DbContexts.BoardContext.SaveChanges();
        return ResponseGenerator.Ok(value: DbContexts.BoardContext.Boards.ToList());
    }

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult DeleteBoards(ulong OrganizationId,
        ulong ProjectId, ulong BoardId)
    {
        var table = DbContexts.BoardContext.Boards;
        if (!DbContexts.ProjectContext.Projects.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var entities = table.Where(x => x.id == BoardId && x.ProjectId == ProjectId);
        if (!entities.Any())
            return ResponseGenerator.NotFound();
        table.RemoveRange(entities);
        DbContexts.BoardContext.SaveChanges();
        return ResponseGenerator.Ok(value: table.ToList());
    }

    #endregion

    #region Columns

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns")]
    public IActionResult GetColumns(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        return ResponseGenerator.Ok(value: col.Where(x => x.BoardId == BoardId).ToList());
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId);
        if(!col.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: col.Single());
    }
    
    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult DeleteColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var colCtx = DbContexts.ColumnContext;
        var col = colCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId);
        colCtx.RemoveRange(col);
        colCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }
    
    [HttpPut("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult PutColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, Column Column)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        if(Column.BoardId != BoardId)
            return ResponseGenerator.NotFound();
        var colCtx = DbContexts.ColumnContext;
        colCtx.Add(Column);
        colCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }

    #endregion

    #region Tasks

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks")]
    public IActionResult GetTasks(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var task = DbContexts.TaskContext.Tasks;

        return ResponseGenerator.Ok(value: task.Where(x => x.ColumnId == ColumnId).ToList());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult GetTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var taskCtx = DbContexts.TaskContext;
        var task = taskCtx.Tasks.Where(x => x.ColumnId == ColumnId && x.Id == TaskId);
        if (!task.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: task.Single());
    }

    
    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult DeleteTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var taskCtx = DbContexts.TaskContext;
        var task = taskCtx.Tasks.Where(x => x.ColumnId == ColumnId && x.Id == TaskId);
        if (!task.Any())
            return ResponseGenerator.NotFound();
        taskCtx.RemoveRange(task);
        return ResponseGenerator.Ok(value: taskCtx.Tasks.ToList());
    }
    
    [HttpPut("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks")]
    public IActionResult PutTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, Task task)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var taskCtx = DbContexts.TaskContext;
        if (task.ColumnId != ColumnId)
            return ResponseGenerator.NotFound();
        taskCtx.Tasks.Add(task);
        taskCtx.SaveChanges();
        return ResponseGenerator.Ok(value: taskCtx.Tasks.Where(x => x.ColumnId == ColumnId).ToList());
    }
    
    #endregion

    #region Components

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components")]
    public IActionResult GetComponents(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var task = DbContexts.TaskContext.Tasks;
        if (!task.Any(x => x.ColumnId == ColumnId && x.Id == TaskId))
            return ResponseGenerator.NotFound();

        var componentCtx = DbContexts.ComponentContext.Components;

        return ResponseGenerator.Ok(value: componentCtx.Where(x => x.TaskId == TaskId).ToList());
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var task = DbContexts.TaskContext.Tasks;
        if (!task.Any(x => x.ColumnId == ColumnId && x.Id == TaskId))
            return ResponseGenerator.NotFound();
        var componentCtx = DbContexts.ComponentContext.Components;
        var component = componentCtx.Where(x => x.TaskId == TaskId && x.id == ComponentId);
        if (!component.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: component.Single());
    }

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult DeleteComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var task = DbContexts.TaskContext.Tasks;
        if (!task.Any(x => x.ColumnId == ColumnId && x.Id == TaskId))
            return ResponseGenerator.NotFound();
        var componentCtx = DbContexts.ComponentContext;
        var component = componentCtx.Components.Where(x => x.TaskId == TaskId && x.id == ComponentId);
        if (!component.Any())
            return ResponseGenerator.NotFound();
        componentCtx.Components.RemoveRange(component);
        componentCtx.SaveChanges();
        return ResponseGenerator.Ok(value: componentCtx.Components.Where(x=> x.TaskId == TaskId).ToList());
    }
    
    [HttpPut("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components")]
    public IActionResult PutComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ComponentDto componentDto)
    {
        var prj = DbContexts.ProjectContext.Projects;
        if (!prj.Any(x => x.OrganizationId == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var board = DbContexts.BoardContext.Boards;
        if (!board.Any(x => x.ProjectId == ProjectId && x.id == BoardId))
            return ResponseGenerator.NotFound();
        var col = DbContexts.ColumnContext.Columns;
        if (!col.Any(x => x.BoardId == BoardId && x.Id == ColumnId))
            return ResponseGenerator.NotFound();
        var task = DbContexts.TaskContext.Tasks;
        if (!task.Any(x => x.ColumnId == ColumnId && x.Id == TaskId))
            return ResponseGenerator.NotFound();
        var componentCtx = DbContexts.ComponentContext;
        if (componentDto.TaskId != TaskId)
            return ResponseGenerator.NotFound();
        componentCtx.Components.Add(componentDto);
        componentCtx.SaveChanges();
        return ResponseGenerator.Ok(value: componentCtx.Components
            .Where(x => x.TaskId == TaskId).ToList());
    }

    #endregion
}