namespace Organization.Models.Organization.Project.Component;

public interface IComponent
{
    /// <summary>
    /// Уникальный идентефикатор компонента
    /// </summary>
    long Id { get; init; }
    string Name { get; set; }
}