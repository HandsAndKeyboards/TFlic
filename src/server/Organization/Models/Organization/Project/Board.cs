namespace Organization.Models.Organization.Project;

public class Board
{
    /// <summary>
    /// Название доски
    /// </summary>
    string Name { get; set; } = null!;

    /// <summary>
    /// Столбцы доски
    /// </summary>
    private List<Column> Columns { get; set; }

    public void CreateColumn(string name)
    {
        Columns.Add(new Column{Name = name});
    }
}