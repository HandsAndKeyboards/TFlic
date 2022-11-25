CREATE TABLE boards (
    id BIGSERIAL PRIMARY KEY,
    project_id BIGINT references projects(id),
    name VARCHAR(50) NOT NULL
)