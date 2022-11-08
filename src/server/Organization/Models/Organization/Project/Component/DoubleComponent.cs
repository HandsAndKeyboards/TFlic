namespace Organization.Models.Organization.Project.Component;

public class DoubleComponent : IComponent
{
    public double Value { get; set; }
    public long Id { get; init; }
    public string Name { get; set; }
}