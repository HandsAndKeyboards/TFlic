CREATE TABLE UserGroups (
    "Id" INTEGER PRIMARY KEY,
    "OrganizationId" INTEGER NOT NULL,
    "Name" VARCHAR(50),

    FOREIGN KEY ("OrganizationId") REFERENCES Organization ("Id") ON DELETE CASCADE ON UPDATE CASCADE
)