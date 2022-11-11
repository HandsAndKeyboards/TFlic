CREATE TABLE UserGroupsAccounts (
    "UserGroupId" INTEGER,
    "AccountId" INTEGER,

    FOREIGN KEY ("UserGroupId") REFERENCES UserGroups ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ("AccountId") REFERENCES UserGroups ("Id") ON DELETE CASCADE ON UPDATE CASCADE
)