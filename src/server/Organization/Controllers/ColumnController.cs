using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
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
            return ResponseGenerator.NotFound();
        var columnsDto = columns.Select(x => new DTO.Column(x)).ToList();
        return ResponseGenerator.Ok(value: columnsDto);
    }

    [HttpGet("Columns/{ColumnId}")]
    public IActionResult GetColumn(ulong OrganizationId, ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var ColCtx = DbContexts.GetNotNull<ColumnContext>();
        var columns = ColCtx.Columns.Where(x => x.BoardId == BoardId && x.Id == ColumnId)
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
            return ResponseGenerator.NotFound();
        
        if (!columns.All(x => x.Board.ProjectId == ProjectId && x.Board.Project.OrganizationId == OrganizationId))
            return ResponseGenerator.NotFound();
        ColCtx.RemoveRange(ColCtx.Columns.Where(x => x.Id == ColumnId)
            .Include(x => x.Tasks)
            .ThenInclude(x => x.Components));
        ColCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }
    #endregion

    #region PUT
    [HttpPut("Columns")]
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
}