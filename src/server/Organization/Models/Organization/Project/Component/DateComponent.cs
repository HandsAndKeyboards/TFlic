namespace Organization.Models.Organization.Project.Component;

public class DateComponent: IComponent
{
    public DateTime value { get; set; }
    public long Id { get; init; }
    public string Name { get; set; }
}