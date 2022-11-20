CREATE TABLE columns (
    id BIGSERIAL PRIMARY KEY,
    board_id BIGINT references boards(id),
    position INT NOT NULL,
    name VARCHAR(50) NOT NULL
)