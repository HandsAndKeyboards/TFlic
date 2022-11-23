using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

namespace Organization.Controllers;

[ApiController]
[Route("Organizations/{OrganizationId}/Projects/{ProjectId}")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class BoardController : ControllerBase
{
    private readonly ILogger<BoardController> _logger;

    public BoardController(ILogger<BoardController> logger)
    {
        _logger = logger;
    }

    #region GET
    [HttpGet("Boards")]
    public IActionResult GetBoards(ulong OrganizationId, ulong ProjectId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var boards = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId)
            .Include(x => x.Project)
            .Include(x => x.Columns)
            .ToList();
        if (boards.Any(x => x.Project.OrganizationId != OrganizationId))
            return ResponseGenerator.NotFound();
        var boardsDto = boards.Select(x => new DTO.Board(x)).ToList();
        return ResponseGenerator.Ok(value: boardsDto);
    }
    
    [HttpGet("Boards/{BoardId}")]
    public IActionResult GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var boards = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Include(x => x.Project)
            .Include(x => x.Columns)
            .ToList();
        if (boards.Any(x => x.Project.OrganizationId != OrganizationId))
            return ResponseGenerator.NotFound();
        var boardsDto = boards.Select(x => new DTO.Board(x)).ToList();
        if (!boardsDto.Any())
            return ResponseGenerator.NotFound();
        return ResponseGenerator.Ok(value: boardsDto.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Boards/{BoardId}")]
    public IActionResult DeleteBoards(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var columns = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Include(x => x.Project).ToList();
        if (!columns.Any() && columns.Any(x => x.Project.OrganizationId != OrganizationId))
            return ResponseGenerator.NotFound();
        
        BoardCtx.RemoveRange(BoardCtx.Boards.Where(x => x.id == BoardId)
            .Include(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components));
        BoardCtx.SaveChanges();
        return ResponseGenerator.Ok();
    }
    #endregion
    
    #region PUT
    [HttpPut("Boards")]
    public IActionResult GetBoards(ulong OrganizationId, ulong ProjectId, Board board)
    {
        using var ctx = DbContexts.Get<BoardContext>();
        if(ctx == null)
            return ResponseGenerator.NotFound();   
        ctx.Boards.Add(board);
        ctx.SaveChanges();
        return ResponseGenerator.Ok(value: ctx.Boards.Where(x => x.ProjectId == ProjectId).ToList());
    }

    #endregion
}
