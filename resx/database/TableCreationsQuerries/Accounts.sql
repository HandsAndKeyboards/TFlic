CREATE TABLE Accounts (
    "Id" INTEGER,
    "Name" VARCHAR(50) NOT NULL,
    "Login" VARCHAR(50) NOT NULL,
    "PasswordHash" BLOB NOT NULL,

    PRIMARY KEY ("Id")
)