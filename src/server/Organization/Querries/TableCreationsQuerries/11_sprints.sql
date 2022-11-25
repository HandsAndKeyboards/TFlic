CREATE TABLE sprints (
    id BIGSERIAL    PRIMARY KEY,
    organisation_id BIGINT  references organizations(id),
    project_id      BIGINT  references projects(id),
    
    begin_date      DATE    NOT NULL,
    duration        INT     NOT NULL,
    number          BIGINT  NOT NULL
)