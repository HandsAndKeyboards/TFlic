CREATE TABLE Tasks (
    "Id" BIGSERIAL PRIMARY KEY,
    "ColumnId" BIGINT references Columns("Id"),
    "LogId" BIGINT references Log("Id"),

    "Position" INT NOT NULL,
    "Name" VARCHAR(50) NOT NULL,
    "Description" TEXT NOT NULL,
    "CreationTime" DATE NOT NULL,
    "Status" VARCHAR(20) NOT NULL
)