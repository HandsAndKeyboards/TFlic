namespace Organization.Controllers.DTO.POST;

public record ColumnDTO
{
    public string Name { get; init; }
    public int Position { get; init; }
    public int LimitOfTask { get; init; }
}