using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.GET;
using Organization.Controllers.DTO.POST;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

namespace Organization.Controllers;

#if AUTH
using Models.Authentication;
using Microsoft.AspNetCore.Authorization;
[Authorize]
#endif
[ApiController]
[Route("Organizations/{OrganizationId}/Projects/{ProjectId}/Boards/{BoardId}")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ColumnController : ControllerBase
{
    private readonly ILogger<ColumnController> _logger;

    public ColumnController(ILogger<ColumnController> logger)
    {
        _logger = logger;
    }

    #region GET
    [HttpGet("Columns")]
    public ActionResult<ICollection<ColumnGET>> GetColumns(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsColumnPathCorrect(OrganizationId, ProjectId, BoardId))
            return NotFound();
        using var ctx = DbContexts.Get<ColumnContext>();
        var cmp = ContextIncluder.GetColumn(ctx)
            .Include(x => x.Tasks)
            .Where(x => x.BoardId == BoardId)
            .Select(x => new ColumnGET(x))
            .ToList();
        return Ok(cmp);
    }

    [HttpGet("Columns/{ColumnId}")]
    public ActionResult<ColumnGET> GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsColumnPathCorrect(OrganizationId, ProjectId, BoardId))
            return NotFound();
        using var ctx = DbContexts.Get<ColumnContext>();
        var cmp = ContextIncluder.GetColumn(ctx)
            .Include(x => x.Tasks)
            .Where(x => x.BoardId == BoardId && x.Id == ColumnId)
            .Select(x => new ColumnGET(x))
            .ToList();
        return !cmp.Any() ? NotFound() : Ok(cmp.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Columns/{ColumnId}")]
    public ActionResult DeleteColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsColumnPathCorrect(OrganizationId, ProjectId, BoardId))
            return NotFound();
        using var ctx = DbContexts.Get<ColumnContext>();
        var cmp = ContextIncluder.GetColumn(ctx).Where(x => x.Id == ColumnId && x.BoardId == BoardId);
        ctx.Columns.RemoveRange(cmp);
        ctx.SaveChanges();
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("Columns")]
    public ActionResult PostColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ColumnDTO Column)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsColumnPathCorrect(OrganizationId, ProjectId, BoardId))
            return NotFound();
        
        using var ctx = DbContexts.Get<ColumnContext>();
        var obj = new Column()
        {
            
            Name = Column.Name,
            Position = Column.Position,
            LimitOfTask = Column.LimitOfTask,
            BoardId = BoardId
        };
        ctx.Columns.Add(obj);
        ctx.SaveChanges();
        return Ok(obj);
    }
    #endregion
    
    #region PATCH
    [HttpPatch("Columns/{ColumnId}")]
    public ActionResult<ColumnGET> PatchColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId,
        [FromBody] JsonPatchDocument<Column> patch)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsColumnPathCorrect(OrganizationId, ProjectId, BoardId))
            return NotFound();
        using var ctx = DbContexts.Get<ColumnContext>();
        var obj = ContextIncluder.GetColumn(ctx).Where(x => x.Id == ColumnId && x.BoardId == BoardId).ToList();
        patch.ApplyTo(obj.Single());
        ctx.SaveChanges();
        
        return Ok(new ColumnGET(obj.Single()));
    }
    #endregion
}