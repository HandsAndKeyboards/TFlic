using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.GET;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Prj = Organization.Models.Organization.Project;
using postDTO = Organization.Controllers.DTO.POST.ProjectDTO;

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
    public ActionResult<ICollection<ProjectGET>> GetProjects(ulong OrganizationId)
    {
        using var ctx = DbContexts.Get<ProjectContext>();
        var projects = ContextIncluder.GetProject(ctx)
            .Where(x => x.OrganizationId == OrganizationId)
            .ToList();
        var projectsDto = projects.Select(x => new ProjectGET(x)).ToList();
        return Ok(projectsDto);
    }
    
    [HttpGet("Projects/{ProjectId}")]
    public ActionResult<ProjectGET> GetProject(ulong OrganizationId, ulong ProjectId)
    {
        using var ctx = DbContexts.Get<ProjectContext>();
        var projects = ContextIncluder.GetProject(ctx)
            .Where(x => x.OrganizationId == OrganizationId)
            .ToList();
        var projectsDto = projects.Select(x => new ProjectGET(x)).ToList();
        if (!projectsDto.Any())
            return NotFound();
        return Ok(projectsDto.Single());
    }
    
    #endregion

    #region DELETE
    [HttpDelete("Projects/{ProjectId}")]
    public ActionResult DeleteProjects(ulong OrganizationId, ulong ProjectId)
    {
        using var ProjectCtx = DbContexts.Get<ProjectContext>();
        var projects = ContextIncluder.DeleteProject(ProjectCtx)
            .Where(x => x.OrganizationId == OrganizationId && x.id == ProjectId)
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
    public ActionResult PostProjects(ulong OrganizationId, postDTO project)
    {
        using var orgCtx = DbContexts.Get<OrganizationContext>();
        if (!orgCtx.Organizations.Any(x => x.Id == OrganizationId))
            return NotFound();
        using var ctx = DbContexts.Get<ProjectContext>();
        ctx.Projects.Add(new Models.Organization.Project.Project{name = project.Name, OrganizationId = OrganizationId});
        ctx.SaveChanges();
        return Ok();
    }
    #endregion

    #region PATCH
    [HttpPatch("Projects/{ProjectId}")]
    public ActionResult<ProjectGET> PatchProject(ulong OrganizationId, ulong ProjectId, [FromBody] JsonPatchDocument<Prj.Project> patch)
    {
        using var ctx = DbContexts.Get<ProjectContext>();

        var obj = ContextIncluder.GetProject(ctx)
            .Where(x => x.id == ProjectId)
            .ToList();
        
        if (!PathChecker.IsProjectPathCorrect(obj, OrganizationId))
            return NotFound();
        
        patch.ApplyTo(obj.Single());
        ctx.SaveChanges();
        
        return Ok(new ProjectGET(obj.Single()));
    }
    #endregion
}
