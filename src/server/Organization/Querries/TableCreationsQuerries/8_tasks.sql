CREATE TABLE tasks (
    id BIGSERIAL PRIMARY KEY,
    column_id BIGINT references columns(id),
    --"LogId" BIGINT references Log("Id"),

    position INT NOT NULL,
    name VARCHAR(50) NOT NULL,
    description TEXT NOT NULL,
    creation_time DATE NOT NULL,
    status VARCHAR(20) NOT NULL
)