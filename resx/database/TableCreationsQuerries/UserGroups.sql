create table UserGroups (
    "GlobalId" bigserial primary key, -- id в таблице
    "LocalId" bigint not null, -- id в конкретной организации
    "OrganizationId" bigint not null,
    "Name" varchar(50),

    foreign key ("OrganizationId") references Organizations ("Id") on delete cascade on update cascade
)