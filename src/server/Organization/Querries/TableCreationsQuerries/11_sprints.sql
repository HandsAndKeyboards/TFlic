CREATE TABLE sprints (
    id BIGSERIAL    PRIMARY KEY,
    organization_id BIGINT  references organizations(id),
    project_id      BIGINT  references projects(id),
    
    begin_date      DATE    NOT NULL,
    end_date      DATE    NOT NULL,
    duration        INT     NOT NULL,
    number          BIGINT  NOT NULL
)