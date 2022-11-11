namespace Organization.Models.Organization.Project.Component;

public class DoubleComponent : IComponent
{
    private double _value { get; set; }
    public long Id { get; init; }
    public string Name { get; set; } = String.Empty;
    public object Value
    {
        get => _value;
        set => _value = (double)value;
    } 
}