create table accounts (
    id bigserial,
    name varchar(50) not null,
    login varchar(50) not null unique,
    password_hash varchar(44) not null,

    primary key (id)
)