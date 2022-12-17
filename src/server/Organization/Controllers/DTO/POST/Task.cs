namespace Organization.Controllers.DTO.POST;

public record TaskDTO
{
    public int Position { get; init; }
    public string Name { get; init; }

    public string Description { get; init; }
    public DateTime CreationTime { get; init; }
    public string Status { get; init; }

    public ulong id_executor { get; init; }
    
    public uint priority { get; init; }
    public DateTime Deadline { get; init; }
}