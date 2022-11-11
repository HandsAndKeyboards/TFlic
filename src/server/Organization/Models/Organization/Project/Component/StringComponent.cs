namespace Organization.Models.Organization.Project.Component;

public class StringComponent: IComponent
{
    private string _value { get; set; } = string.Empty;
    public long Id { get; init; }
    public string Name { get; set; } = string.Empty;
    
    public object Value
    {
        get => _value;
        set => _value = (string)value;
    } 
}