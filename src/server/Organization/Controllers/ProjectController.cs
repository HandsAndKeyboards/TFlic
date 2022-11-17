using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.Common;
using Organization.Models.Organization.Project;
using Organization.Models.Organization.Project.Component;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers;

//TODO Замени заглушки на ответы от DB
[ApiController]
[Route("Organizations/{OrganizationId}")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Projects")]
    public IActionResult GetProjects(long OrganizationId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new List<IProject>());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards")]
    public IActionResult GetBoards(long OrganizationId, long ProjectId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new List<Board>());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}")]
    public IActionResult GetBoard(long OrganizationId, long ProjectId, long BoardId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new Board());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns")]
    public IActionResult GetColumns(long OrganizationId, long ProjectId, long BoardId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new List<Column>());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}")]
    public IActionResult GetColumn(long OrganizationId, long ProjectId, long BoardId, long ColumnId)
    {
        //TODO 
        return ResponseGenerator.Ok(value: new Column());
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks")]
    public IActionResult GetTasks(long OrganizationId, long ProjectId, long BoardId, long ColumnId)
    {
        //TODO 
        return ResponseGenerator.Ok(
             value: new List<Task>()
            );
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}")]
    public IActionResult GetTask(long OrganizationId, long ProjectId, long BoardId, long ColumnId, long TaskId)
    {
        //TODO 
        return ResponseGenerator.Ok(
            value: new Task
            {
                Name = "Task", Id = TaskId, Components = new List<IComponent>
                {
                    new StringComponent { Name = "Name", Value = "tmpValue", Id = 0 },
                    new DateComponent { Name = "DateComponent", Id = 1, Value = DateTime.Now }
                }
            });
    }

    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Columns/{ColumnId}/Tasks/{TaskId}/Components")]
    public IActionResult GetComponents(long OrganizationId, long ProjectId, long BoardId, long ColumnId, long TaskId)
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
    public IActionResult GetComponent(long OrganizationId, long ProjectId, long BoardId, long ColumnId, long TaskId, long ComponentId)
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
    public IActionResult DeleteComponent(long OrganizationId, long ProjectId, long BoardId, long ColumnId, long TaskId, long ComponentId)
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