using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;
using Organization.Models.Organization.Project.Component;
using Org = Organization.Models.Organization.Organization;
using Project = Organization.Models.Organization.Project.Project;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Controllers.Service;

public static class ContextIncluder
{
    public static IIncludableQueryable<ComponentDto, Org> GetComponent(ComponentContext ctx)
    {
        return ctx.Components
            .Include(x => x.Task)
            .ThenInclude(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Organization);
    }

    public static IIncludableQueryable<Task, Org> GetTask(TaskContext ctx)
    {
        return ctx.Tasks
            .Include(x => x.Column)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Organization);
    }

    public static IIncludableQueryable<Column, Org> GetColumn(ColumnContext ctx)
    {
        return ctx.Columns
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Organization);
    }

    public static IIncludableQueryable<Board, Org> GetBoard(BoardContext ctx)
    {
        return ctx.Boards
            .Include(x => x.Project)
            .ThenInclude(x => x.Organization);
    }

    public static IIncludableQueryable<Project, Org> GetProject(ProjectContext ctx)
    {
        return ctx.Projects
            .Include(x => x.Organization);
    }

    public static DbSet<Task> DeleteTask(TaskContext ctx)
    {
        return ctx.Tasks;
        //.Include(x => x.Components);
    }

    public static IIncludableQueryable<Column, ICollection<ComponentDto>> DeleteColumn(ColumnContext ctx)
    {
        return ctx.Columns
            .Include(x => x.Tasks)
            .ThenInclude(x => x.Components);
    }

    public static IIncludableQueryable<Board, ICollection<ComponentDto>> DeleteBoard(BoardContext ctx)
    {
        return ctx.Boards
            .Include(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components);
    }

    public static IIncludableQueryable<Project, ICollection<ComponentDto>> DeleteProject(ProjectContext ctx)
    {
        return ctx.Projects
            .Include(x => x.boards)
            .ThenInclude(x => x.Columns)
            .ThenInclude(x => x.Tasks)
            .ThenInclude(x => x.Components);
    }
    
    public static DbSet<ComponentDto> DeleteComponent(ComponentContext ctx)
    {
        return ctx.Components;
    }
}
