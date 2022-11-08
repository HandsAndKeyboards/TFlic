﻿namespace Organization.Models.Organization.Project.Component;

public class StringComponent: IComponent
{
    public string value { get; set; }
    public long Id { get; init; }
    public string Name { get; set; }
}