create table auth_info (
    account_id bigint,
    login varchar(50) unique not null,
    password_hash varchar(44) not null,
    refresh_token text,
    
    primary key (account_id, login),
    foreign key (account_id) references accounts (id) on delete cascade on update cascade
)