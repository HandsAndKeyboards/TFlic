create table organizations (
    id bigserial,
    name varchar(50) not null,
    description text,

    primary key(id)
)