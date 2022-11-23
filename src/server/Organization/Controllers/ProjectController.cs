using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

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

    #region GET
    
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
    
    #endregion

    #region DELETE
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
    #endregion

    #region PUT
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
}