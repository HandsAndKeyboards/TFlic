CREATE TABLE tasks (
    id BIGSERIAL PRIMARY KEY,
    column_id BIGINT references columns(id),
    --"LogId" BIGINT references Log("Id"),

    position INT NOT NULL,
    name VARCHAR(50) NOT NULL,
    description TEXT NOT NULL,
    creation_time DATE NOT NULL,
    status VARCHAR(20) NOT NULL,
    priority INT NOT NULL default(1),
    id_executor BIGINT references accounts(id),
    deadline DATE NOT NULL,
    estimated_time INT NOT NULL default(0)
)