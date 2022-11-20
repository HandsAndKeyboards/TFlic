namespace Organization.Models.Organization.Project.Component;

public class SectionComponent: IComponent
{
    public ulong Id { get; init; }
    public string Name { get; set; } = string.Empty;
    
    private List<IComponent> _components = new();

    public object Value
    {
        get => _components;
        set => _components = (List<IComponent>)value;
    } 
}