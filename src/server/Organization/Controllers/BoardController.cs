using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO.GET;
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
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsBoardPathCorrect(OrganizationId, ProjectId))
            return NotFound();
        using var ctx = DbContexts.Get<BoardContext>();
        var cmp = ContextIncluder.GetBoard(ctx)
            .Include(x => x.Columns)
            .Where(x => x.ProjectId == ProjectId)
            .Select(x => new BoardGET(x))
            .ToList();
        return Ok(cmp);
    }
    
    [HttpGet("Boards/{BoardId}")]
    public ActionResult<BoardGET> GetBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsBoardPathCorrect(OrganizationId, ProjectId))
            return NotFound();
        using var ctx = DbContexts.Get<BoardContext>();
        var cmp = ContextIncluder.GetBoard(ctx)
            .Include(x => x.Columns)
            .Where(x => x.ProjectId == ProjectId && x.id == BoardId)
            .Select(x => new BoardGET(x))
            .ToList();
        return !cmp.Any() ? NotFound() : Ok(cmp.Single());
    }
    #endregion

    #region DELETE
    [HttpDelete("Boards/{BoardId}")]
    public ActionResult DeleteBoards(ulong OrganizationId, ulong ProjectId, ulong BoardId)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsBoardPathCorrect(OrganizationId, ProjectId))
            return NotFound();
        using var ctx = DbContexts.Get<BoardContext>();
        var cmp = ContextIncluder.DeleteBoard(ctx).Where(x => x.id == BoardId && x.ProjectId == ProjectId);
        ctx.Boards.RemoveRange(cmp);
        ctx.SaveChanges();
        return Ok();
    }
    #endregion
    
    #region POST
    [HttpPost("Boards")]
    public ActionResult PostBoards(ulong OrganizationId, ulong ProjectId, DTO.POST.BoardDTO board)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsBoardPathCorrect(OrganizationId, ProjectId))
            return NotFound();
        
        using var ctx = DbContexts.Get<BoardContext>();
        var obj = new Board
        {
            Name = board.Name,
            ProjectId = ProjectId
        };
        obj.Columns.Add(new Column{Position = 0, LimitOfTask = 0, Name = "backlog"});
        ctx.Boards.Add(obj);
        ctx.SaveChanges();
        return Ok();
    }

    #endregion
    
    #region PATCH
    [HttpPatch("Boards/{BoardId}")]
    public ActionResult<BoardGET> PatchBoard(ulong OrganizationId, ulong ProjectId, ulong BoardId, [FromBody] JsonPatchDocument<Board> patch)
    {
#if AUTH
        var token = TokenProvider.GetToken(Request);
        if (!AuthenticationManager.Authorize(token, OrganizationId, adminRequired: true)) { return Forbid(); }
#endif
        if (!PathChecker.IsBoardPathCorrect(OrganizationId, ProjectId))
            return NotFound();
        using var ctx = DbContexts.Get<BoardContext>();
        var obj = ContextIncluder.GetBoard(ctx).Where(x => x.id == BoardId && x.ProjectId == ProjectId).ToList();
        patch.ApplyTo(obj.Single());
        ctx.SaveChanges();
        
        return Ok(new BoardGET(obj.Single()));
    }
    #endregion
}
