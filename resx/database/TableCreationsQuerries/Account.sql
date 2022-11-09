CREATE TABLE Account (
    "Id" INTEGER,
    "Login" VARCHAR(50) NOT NULL,
    "PasswordHash" BLOB NOT NULL,
    "Name" VARCHAR(50) NOT NULL,

    PRIMARY KEY ("Id")
)