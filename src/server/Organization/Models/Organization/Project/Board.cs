using System.ComponentModel.DataAnnotations;

namespace Organization.Models.Organization.Project;

public class Board
{
    /// <summary>
    /// Уникальный идентификатор доски
    /// </summary>
    public ulong Id { get; init; }

    /// <summary>
    /// Название доски
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Столбцы доски
    /// </summary>
    public ICollection<Column> Columns
    {
        get => _columns;
        init => _columns = (List<Column>)value;
    }

    private readonly List<Column> _columns = new();

    public void CreateColumn(string name)
    {
        //TODO выдача столбцам уникального id
        Columns.Add(new Column{Name = name, Position = Columns.Count});
    }

    public bool RemoveColumn(ulong id)
    {
        if (!ContainColumn(id))
            return false;
        var column = GetColumn(id);
        return Columns.Remove(column);
    }

    public bool MoveColumn(ulong id, int position)
    {
        if (Columns.Count <= position || !ContainColumn(id))
            return false;
        var targetColumn = GetColumn(id);
        targetColumn.Position = position;
        foreach (var item in Columns.Where(column => column.Position >= targetColumn.Position))
            item.Position--;
        return true;
    }

    public bool ContainColumn(ulong id)
    {
        return Columns.Any(column => column.Id == id);
    }

    public bool MoveTask(ulong columnId, ulong taskId, int position)
    {
        if (!ContainColumn(columnId))
            return false;
        var targetColumn = GetColumn(columnId);
        return targetColumn.MoveTask(taskId, position);
    }

    public bool AddTask(ulong columnId, Task task)
    {
        if (!ContainColumn(columnId))
            return false;
        var targetColumn = GetColumn(columnId);
        return targetColumn.AddTask(task);
    }

    public Column GetColumn(ulong id)
    {
        return Columns.Single(column => column.Id == id);
    }
}