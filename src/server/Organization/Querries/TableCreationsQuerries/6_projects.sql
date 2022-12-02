CREATE TABLE projects (
    id BIGSERIAL PRIMARY KEY,
    organization_id BIGINT references organizations(id),
    is_archived BOOLEAN DEFAULT FALSE,
    --"LogId" BIGINT references Log("Id"),
    name VARCHAR(50) NOT NULL
)