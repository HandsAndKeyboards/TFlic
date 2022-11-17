CREATE TABLE Projects (
    "Id" BIGSERIAL PRIMARY KEY,
    "OrganizationId" BIGINT references Organizations("Id"),
    "LogId" BIGINT references Log("Id"),
    "Name" VARCHAR(50) NOT NULL
)