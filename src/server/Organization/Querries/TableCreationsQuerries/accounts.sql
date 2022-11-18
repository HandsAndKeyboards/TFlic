create table accounts (
    id bigserial,
    name varchar(50) not null,
    login varchar(50) not null,
    password_hash bytea not null,

    primary key (id)
)