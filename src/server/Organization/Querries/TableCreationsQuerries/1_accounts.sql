create table accounts (
    id bigserial unique,
    name varchar(50) not null,
--     login varchar(50) unique,
--     password_hash varchar(44) not null,

    primary key (id)
)