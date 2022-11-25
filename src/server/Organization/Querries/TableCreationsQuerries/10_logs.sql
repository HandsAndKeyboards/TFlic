CREATE TABLE logs (
    id BIGSERIAL PRIMARY KEY,

    -- ����� �� ������� ������������ ����?
    organization_id BIGINT references organizations(id),
    project_id BIGINT references projects(id),
    task_id             BIGINT NOT NULL,
    old_estimated_time  BIGINT NOT NULL,
    new_estimated_time  BIGINT NOT NULL,
    real_time           BIGINT NOT NULL,
    sprint_number       BIGINT NOT NULL,
    edit_date           DATE   NOT NULL
)