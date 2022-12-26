CREATE TABLE logs (
    id BIGSERIAL PRIMARY KEY,

    -- Нужно ли хранить организейтин айди?
    organization_id BIGINT references organizations(id),
    project_id BIGINT references projects(id),
    task_id             BIGINT NOT NULL,
    old_estimated_time  BIGINT ,
    new_estimated_time  BIGINT NOT NULL,
    real_time           BIGINT ,
    sprint_number       BIGINT NOT NULL,
    edit_date           DATE   NOT NULL,
    accountChanged_id   INT    NOT NULL
)