create table Accounts (
    "Id" bigserial,
    "Name" varchar(50) not null,
    "Login" varchar(50) not null,
    "PasswordHash" bytea not null,

    primary key ("Id")
)