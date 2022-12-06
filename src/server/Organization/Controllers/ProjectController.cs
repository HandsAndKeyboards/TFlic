using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Models.Contexts;
using Prj = Organization.Models.Organization.Project;
using Project = Organization.Controllers.DTO.POST.ProjectDTO;

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
        var projectsDto = projects.Select(x => new DTO.GET.Project(x)).ToList();
        return Ok(projectsDto);
    }
    
    [HttpGet("Projects/{ProjectId}")]
    public IActionResult GetProject(ulong OrganizationId, ulong ProjectId)
    {
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        var projects = ProjectCtx.Projects.
            Where(x => x.OrganizationId == OrganizationId && x.id == ProjectId)
            .Include(x => x.boards)
            .ToList();
        var projectsDto = projects.Select(x => new DTO.GET.Project(x)).ToList();
        if (!projectsDto.Any())
            return NotFound();
        return Ok(projectsDto.Single());
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
            return NotFound();
        ProjectCtx.RemoveRange(projects);
        ProjectCtx.SaveChanges();
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("Projects")]
    public IActionResult PostProjects(ulong OrganizationId, DTO.POST.ProjectDTO project)
    {
        using var orgCtx = DbContexts.Get<OrganizationContext>();
        if (!orgCtx.Organizations.Any(x => x.Id == OrganizationId))
            return NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        ctx.Projects.Add(new Models.Organization.Project.Project{name = project.Name, OrganizationId = OrganizationId});
        ctx.SaveChanges();
        return Ok(ctx.Projects.ToList());
    }
    #endregion

    #region PATCH
    [HttpPatch("Projects/{ProjectId}")]
    public IActionResult PatchProject(ulong OrganizationId, ulong ProjectId, [FromBody] JsonPatchDocument<Prj.Project> patch)
    {
        using var ctx = DbContexts.Get<ProjectContext>();

        var obj = ctx.Projects
            .Where(x => x.id == ProjectId && x.OrganizationId == OrganizationId)
            .Include(x => x.boards)
            .Single();
        
        patch.ApplyTo(obj);
        ctx.SaveChanges();
        
        return Ok(new DTO.GET.Project(obj));
    }
    #endregion
}
