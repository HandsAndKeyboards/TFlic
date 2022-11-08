namespace Organization.Models.Organization.Project.Component;

public class SectionComponent: IComponent
{
    public long Id { get; init; }
    public string Name { get; set; }
    public ICollection<IComponent> Components { get; set; }
}