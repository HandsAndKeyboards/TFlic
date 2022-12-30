namespace Organization.Controllers.DTO.POST;

public record ComponentDTO
{
    public ulong component_type_id { get; init; }
    public int position { get; init; }
    public string name { get; init; }
    public string value { get; init; }
}