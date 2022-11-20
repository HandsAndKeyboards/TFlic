CREATE TABLE component_type (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(64) NOT NULL
);

CREATE TABLE components (
    id BIGSERIAL PRIMARY KEY,
    component_type_id BIGINT references component_type(id),
    task_id BIGINT references tasks(id),
    position INT NOT NULL,
    name VARCHAR(50) NOT NULL,
    value TEXT NOT NULL
)