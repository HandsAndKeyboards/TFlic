using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.GET;
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
    public ActionResult<ICollection<BoardGET>> GetBoards(ulong OrganizationId, ulong ProjectId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var boards = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId)
            .Include(x => x.Project)
            .Include(x => x.Columns)
            .ToList();
        if (boards.Any(x => x.Project.OrganizationId != OrganizationId))
            return NotFound();
        var boardsDto = boards.Select(x => new DTO.GET.BoardGET(x)).ToList();
        return Ok(boardsDto);
    }
    
    [HttpGet("Boards/{BoardId}")]
    public ActionResult<BoardGET> GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var boards = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Include(x => x.Project)
            .Include(x => x.Columns)
            .ToList();
        if (boards.Any(x => x.Project.OrganizationId != OrganizationId))
            return NotFound();
        var boardsDto = boards.Select(x => new DTO.GET.BoardGET(x)).ToList();
        if (!boardsDto.Any())
            return NotFound();
        return Ok(boardsDto.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Boards/{BoardId}")]
    public ActionResult DeleteBoards(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
        using var BoardCtx = DbContexts.Get<BoardContext>();
        var columns = BoardCtx.Boards.Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Include(x => x.Project).ToList();
        if (!columns.Any() && columns.Any(x => x.Project.OrganizationId != OrganizationId))
            return NotFound();
        
        BoardCtx.RemoveRange(BoardCtx.Boards.Where(x => x.id == BoardId)
            .Include(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components));
        BoardCtx.SaveChanges();
        return Ok();
    }
    #endregion
    
    #region POST
    [HttpPost("Boards")]
    public ActionResult PostBoards(ulong OrganizationId, ulong ProjectId, DTO.POST.BoardDTO board)
    {
        using var prjCtx = DbContexts.Get<ProjectContext>();
        var prj = prjCtx.Projects.Include(x => x.Organization);
        if (!prj.Any(x => x.id == ProjectId && x.OrganizationId == OrganizationId))
            return NotFound();
        
        using var ctx = DbContexts.Get<BoardContext>();
        var obj = new Board {Name = board.Name, ProjectId = ProjectId};
        ctx.Boards.Add(obj);
        ctx.SaveChanges();
        return Ok(obj);
    }

    #endregion
    
    #region PATCH
    [HttpPatch("Boards/{BoardId}")]
    public ActionResult<BoardGET> PatchBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId, [FromBody] JsonPatchDocument<Board> patch)
    {
        using var ctx = DbContexts.Get<BoardContext>();


            var obj = ctx.Boards
                .Include(x => x.Columns)
                .Include(x => x.Project)
                .Single(x => x.id == BoardId && x.ProjectId == ProjectId
                 && x.Project.OrganizationId == OrganizationId);
            
        patch.ApplyTo(obj);
        ctx.SaveChanges();

        return Ok(new BoardGET(obj));
    }
    #endregion
}
