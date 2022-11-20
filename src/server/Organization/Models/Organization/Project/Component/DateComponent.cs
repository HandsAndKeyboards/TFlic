namespace Organization.Models.Organization.Project.Component;

public class DateComponent: IComponent
{
    private DateTime _value { get; set; }
    public ulong Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public object Value
    {
        get => _value;
        set => _value = (DateTime)value;
    } 
}