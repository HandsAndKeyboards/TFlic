create table UserGroupsAccounts (
    "UserGroupId" bigint,
    "AccountId" bigint,

    foreign key ("UserGroupId") references UserGroups ("GlobalId") on delete cascade on update cascade,
    foreign key ("AccountId") references Accounts ("Id") on delete cascade on update cascade
)