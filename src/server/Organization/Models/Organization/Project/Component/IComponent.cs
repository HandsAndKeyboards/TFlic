namespace Organization.Models.Organization.Project.Component;

public interface IComponent
{
    /// <summary>
    /// Уникальный идентификатор компонента
    /// </summary>
    ulong Id { get; init; }
    string Name { get; set; }
    object Value { get; set; }
}