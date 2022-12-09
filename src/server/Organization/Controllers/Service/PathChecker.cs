using System.ComponentModel;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;
using Organization.Models.Organization.Project.Component;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers.Service;


public static class PathChecker
{
    public static bool IsComponentPathCorrect(IEnumerable<ComponentDto> cmp, ulong OrganizationId,
        ulong ProjectId, ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        return cmp.All(x => x.id == TaskId && x.Task.ColumnId == ColumnId &&
                            x.Task.Column.BoardId == BoardId &&
                            x.Task.Column.Board.ProjectId == ProjectId &&
                            x.Task.Column.Board.Project.OrganizationId == OrganizationId);
    }
    
    public static bool IsTaskPathCorrect(IEnumerable<Task> task, ulong OrganizationId,
        ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        return task.Any(x => x.ColumnId == ColumnId && x.Column.BoardId == BoardId
                            && x.Column.Board.ProjectId == ProjectId
                            && x.Column.Board.Project.OrganizationId == OrganizationId);
    }
    
    public static bool IsColumnPathCorrect(IEnumerable<Column> columns, ulong OrganizationId,
        ulong ProjectId, ulong BoardId)
    {
        return columns.Any(x => x.BoardId == BoardId
                             && x.Board.ProjectId == ProjectId
                             && x.Board.Project.OrganizationId == OrganizationId);
    }
    
    public static bool IsBoardPathCorrect(IEnumerable<Board> boards, ulong OrganizationId,
        ulong ProjectId)
    {
        return boards.Any(x => x.id == ProjectId
                                && x.Project.OrganizationId == OrganizationId);
    }
    
    public static bool IsProjectPathCorrect(IEnumerable<Project> projects, ulong OrganizationId)
    {
        return projects.Any(x => x.id == OrganizationId);
    }
    
    /*/////////////////////////////*/
    
    public static bool IsComponentPathCorrect(ulong OrganizationId, ulong ProjectId, 
        ulong BoardId, ulong ColumnId, ulong TaskId)
    {
        using var pathCtx = DbContexts.Get<ComponentContext>();
        var prj = ContextIncluder.GetComponent(pathCtx);
        return IsComponentPathCorrect(prj, OrganizationId, ProjectId, BoardId, ColumnId, TaskId);
    }
    
    public static bool IsTaskPathCorrect(ulong OrganizationId,
        ulong ProjectId, ulong BoardId, ulong ColumnId)
    {
        using var pathCtx = DbContexts.Get<TaskContext>();
        var prj = ContextIncluder.GetTask(pathCtx);
        return IsTaskPathCorrect(prj, OrganizationId, ProjectId, BoardId, ColumnId);
    }
    
    public static bool IsColumnPathCorrect(ulong OrganizationId,
        ulong ProjectId, ulong BoardId)
    {
        using var pathCtx = DbContexts.Get<ColumnContext>();
        var obj = ContextIncluder.GetColumn(pathCtx);
        return IsColumnPathCorrect(obj, OrganizationId, ProjectId, BoardId);
    }
    
    public static bool IsBoardPathCorrect(ulong OrganizationId,
        ulong ProjectId)
    {
        using var pathCtx = DbContexts.Get<BoardContext>();
        var obj = ContextIncluder.GetBoard(pathCtx);
        return IsBoardPathCorrect(obj, OrganizationId, ProjectId);
    }
    
    public static bool IsProjectPathCorrect(ulong OrganizationId)
    {
        using var pathCtx = DbContexts.Get<ProjectContext>();
        var obj = ContextIncluder.GetProject(pathCtx);
        return IsProjectPathCorrect(obj, OrganizationId);
    }
}