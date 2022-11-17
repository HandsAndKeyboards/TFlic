CREATE TABLE ComponentType (
    "Id" BIGSERIAL PRIMARY KEY,
    "Name" VARCHAR(64) NOT NULL
);

CREATE TABLE Components (
    "Id" BIGSERIAL PRIMARY KEY,
    "ComponentTypeId" BIGINT references ComponentType("Id"),
    "TaskId" BIGINT references Tasks("Id"),
    "Position" INT NOT NULL,
    "Name" VARCHAR(50) NOT NULL,
    "Value" TEXT NOT NULL
)