CREATE TABLE Boards (
    "Id" BIGSERIAL PRIMARY KEY,
    "ProjectId" BIGINT references Projects("Id"),
    "Name" VARCHAR(50) NOT NULL
)