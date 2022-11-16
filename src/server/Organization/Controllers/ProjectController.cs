﻿using Microsoft.AspNetCore.Mvc;
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
    public IEnumerable<IProject> GetProjects(long OrganizationId)
    {
        //TODO 
        return new List<IProject>();
    }
    
    [HttpGet("Projects/{ProjectId}/Boards")]
    public IEnumerable<Board> GetBoards(long OrganizationId, long ProjectId)
    {
        //TODO 
        return new List<Board>();
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}")]
    public IEnumerable<Board> GetBoard(long OrganizationId, long ProjectId, long BoardId)
    {
        //TODO 
        return new List<Board>();
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Tasks")]
    public IEnumerable<Board> GetTasks(long OrganizationId, long ProjectId, long BoardId)
    {
        //TODO 
        return new List<Board>();
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Tasks/{TaskId}")]
    public ActionResult<Task> GetTask(long OrganizationId, long ProjectId, long BoardId, long TaskId)
    {
        //TODO 
        return new Task{Name = "Task", Id = TaskId, Components = new List<IComponent>{new StringComponent{Name = "Name", Value = "tmpValue", Id = 0},
            new DateComponent{Name = "DateComponent",Id = 1, Value = DateTime.Now}}};
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Tasks/{TaskId}/Components")]
    public IEnumerable<IComponent> GetComponents(long OrganizationId, long ProjectId, long BoardId, long TaskId)
    {
        //TODO 
        return new List<IComponent>{new StringComponent{Name = "Name", Value = "tmpValue", Id = 0},
            new DateComponent{Name = "DateComponent",Id = 1, Value = DateTime.Now}};
    }
    
    [HttpGet("Projects/{ProjectId}/Boards/{BoardId}/Tasks/{TaskId}/Components/{ComponentId}")]
    public IEnumerable<IComponent> GetComponents(long OrganizationId, long ProjectId, long BoardId, long TaskId, long ComponentId)
    {
        //TODO 
        return new List<IComponent>{new StringComponent{Name = "Name", Value = "tmpValue", Id = 0},
            new DateComponent{Name = "DateComponent",Id = 1, Value = DateTime.Now}};
    }
}