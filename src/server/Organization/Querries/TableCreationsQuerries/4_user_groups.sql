create table user_groups (
    global_id bigserial primary key, -- id в таблице
    local_id smallint not null, -- id в конкретной организации
    organization_id bigint not null,
    name varchar(50),

    foreign key (organization_id) references organizations (id) on delete cascade on update cascade
)