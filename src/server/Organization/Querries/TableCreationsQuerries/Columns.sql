CREATE TABLE Columns (
    "Id" BIGSERIAL PRIMARY KEY,
    "BoardId" BIGINT references Boards("Id"),
    "Position" INT NOT NULL,
    "Name" VARCHAR(50) NOT NULL
)