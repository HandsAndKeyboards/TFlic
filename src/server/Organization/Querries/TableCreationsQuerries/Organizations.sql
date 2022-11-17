create table Organizations (
    "Id" bigserial,
    "Name" varchar(50) not null,
    "Description" text,

    primary key("Id")
)