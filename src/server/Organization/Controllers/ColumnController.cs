using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

namespace Organization.Controllers;

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
    public IActionResult GetColumns(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var ColCtx = DbContexts.Get<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId)
            .Include(x => x.Tasks)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        var columnsDto = columns.Select(x => new DTO.GET.Column(x)).ToList();
        return Ok(columnsDto);
    }

    [HttpGet("Columns/{ColumnId}")]
    public IActionResult GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var ColCtx = DbContexts.Get<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId)
            .Include(x => x.Tasks)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        var columnsDto = columns.Select(x => new DTO.GET.Column(x)).ToList();
        if (!columnsDto.Any())
            return NotFound();
        return Ok(columnsDto.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Columns/{ColumnId}")]
    public IActionResult DeleteColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var ColCtx = DbContexts.Get<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ToList();
        if (!columns.Any())
            return NotFound();
        
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return NotFound();
        ColCtx.RemoveRange(ColCtx.Columns.Where(x => x.Id == ColumnId)
            .Include(x => x.Tasks)
            .ThenInclude(x => x.Components));
        ColCtx.SaveChanges();
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("Columns")]
    public IActionResult PostColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, Column Column)
    {
        using var pathCtx = DbContexts.Get<BoardContext>();
        //var prj = pathCtx.Boards.Include(x => x.Project)
        //    .ThenInclude(x => x.Organization);
        //if (!prj.Any(x => x.id == BoardId && x.ProjectId == ProjectId && x.Project.OrganizationId == OrganizationId))
        //    return NotFound();
        
        using var ctx = DbContexts.Get<ColumnContext>();
        var obj = new Column{Name = Column.Name, BoardId = BoardId, LimitOfTask = Column.LimitOfTask, Position = Column.Position};
        ctx.Columns.Add(obj);
        ctx.SaveChanges();
        return Ok(obj);
    }
    #endregion
    
    #region PATCH
    [HttpPatch("Columns/{ColumnId}")]
    public IActionResult PatchColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId,
        [FromBody] JsonPatchDocument<Column> patch)
    {
        using var ctx = DbContexts.Get<ColumnContext>();

        var obj = ctx.Columns
            .Include(x => x.Tasks)
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .Single(x => x.Id == ColumnId && x.BoardId == BoardId && x.Board.ProjectId == ProjectId
                         && x.Board.Project.OrganizationId == OrganizationId);
        
        patch.ApplyTo(obj);
        ctx.SaveChanges();
        
        return Ok(new DTO.GET.Column(obj));
    }
    #endregion
}