CREATE TABLE UserGroups (
    "GlobalId" INTEGER PRIMARY KEY, -- id в таблице
    "LocalId" INTEGER NOT NULL, -- id в конкретной организации
    "OrganizationId" INTEGER NOT NULL,
    "Name" VARCHAR(50),

    FOREIGN KEY ("OrganizationId") REFERENCES Organizations ("Id") ON DELETE CASCADE ON UPDATE CASCADE
)