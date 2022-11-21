create table user_groups_accounts (
    id bigserial,
    user_group_id bigint,
    account_id bigint,

    primary key (id),
    foreign key (user_group_id) references user_groups (global_id) on delete cascade on update cascade,
    foreign key (account_id) references accounts (id) on delete cascade on update cascade
)