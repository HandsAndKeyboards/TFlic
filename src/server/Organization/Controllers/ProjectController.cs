using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Common;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;
using Organization.Models.Organization.Project.Component;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers;

//TODO Замени заглушки на ответы от DB
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
            .Where(x => x.organization_id == OrganizationId)
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
        var entities = table.Where(x => x.id == ProjectId && x.organization_id == OrganizationId);
        table.RemoveRange(entities);
        DbContexts.ProjectContext.SaveChanges();
        return ResponseGenerator.Ok(value: table.ToList());
    }

    [HttpPut("Projects")]
    public IActionResult PutProjects(ulong OrganizationId, Project project)
    {
        if (project.organization_id != OrganizationId)
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
        //TODO 
        // var context = new BoardContext();
        return ResponseGenerator.Ok(value: DbContexts.BoardContext.Boards.
            Include(x=> x.Columns).
            ThenInclude(x=> x.Tasks).
            ThenInclude(x =>x.Components).
            ToList());
        
        throw new NotImplementedException();
    }

    [HttpPut("Projects/{ProjectId}/Boards")]
    public IActionResult GetBoards(ulong OrganizationId, Board board)
    {
        //TODO 
        DbContexts.BoardContext.Boards.Add(board);
        DbContexts.BoardContext.SaveChanges();
        return ResponseGenerator.Ok(value: DbContexts.BoardContext.Boards.ToList());
    }

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult DeleteBoards(ulong OrganizationId,
        ulong ProjectId, ulong BoardId)
    {
        //TODO 
        var table = DbContexts.BoardContext.Boards;
        if (!DbContexts.ProjectContext.Projects.Any(x => x.organization_id == OrganizationId && x.id == ProjectId))
            return ResponseGenerator.NotFound();
        var entities = table.Where(x => x.id == BoardId && x.ProjectId == ProjectId);
        if (!entities.Any())
            return ResponseGenerator.NotFound();
        table.RemoveRange(entities);
        DbContexts.BoardContext.SaveChanges();
        return ResponseGenerator.Ok(value: table.ToList());
    }

    #endregion


    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        //TODO 
        var table = DbContexts.BoardContext.Boards;
        var entities = table.Where(x => x.id == BoardId).Include(x => x.Columns).ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components);
        return entities.Count() == 1 ? ResponseGenerator.Ok(value: entities.Single()) : ResponseGenerator.NotFound();
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns")]
    public IActionResult GetColumns(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new List<Column>());
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new Column { Id = 0 });
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks")]
    public IActionResult GetTasks(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        //TODO 
        return ResponseGenerator.Ok(
            value: new List<Task>()
        );
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult GetTask(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        //TODO 
        return ResponseGenerator.Ok(
        );
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components")]
    public IActionResult GetComponents(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        //TODO 

        return ResponseGenerator.Ok(value: new List<IComponent>
            {
                new StringComponent { Name = "Name", Value = "tmpValue", Id = 0 },
                new DateComponent { Name = "DateComponent", Id = 1, Value = DateTime.Now }
            }
        );
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult GetComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        //TODO 
        return ResponseGenerator.Ok(
            value: new List<IComponent>
            {
                new StringComponent { Name = "Name", Value = "tmpValue", Id = 0 },
                new DateComponent { Name = "DateComponent", Id = 1, Value = DateTime.Now }
            }
        );
    }

    [HttpDelete("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IActionResult DeleteComponent(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId,
        ulong ComponentId)
    {
        //TODO 
        return ResponseGenerator.Ok(
            value: new List<IComponent>
            {
                new StringComponent { Name = "Name", Value = "tmpValue", Id = 0 },
                new DateComponent { Name = "DateComponent", Id = 1, Value = DateTime.Now }
            }
        );
    }
}