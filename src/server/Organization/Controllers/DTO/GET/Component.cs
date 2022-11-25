namespace Organization.Controllers.DTO.GET;

public class ComponentDto
{
    
    public ComponentDto(Models.Organization.Project.Component.ComponentDto componentDto)
    {
        id = componentDto.id;
        name = componentDto.name;
        position = componentDto.position;
        component_type_id = componentDto.component_type_id;
        value = componentDto.value;
    }
    public ulong id { get;}

    public ulong component_type_id { get;}
    
    public int position { get;}
    public string name { get;}
    public string value { get;}
}