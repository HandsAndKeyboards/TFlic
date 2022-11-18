create table user_groups_accounts (
    user_group_id bigint,
    account_id bigint,

    foreign key (user_group_id) references user_groups (global_id) on delete cascade on update cascade,
    foreign key (account_id) references accounts (id) on delete cascade on update cascade
)