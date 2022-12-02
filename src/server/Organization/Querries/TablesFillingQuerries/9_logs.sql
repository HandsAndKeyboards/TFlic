-- 1 org 1 proj

-- 1 task
insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '1', '120', '60', '1', '1', '2022-11-21');

insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '1', '60', '62', '1', '1', '2022-11-24');

insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '1', '62', '0', '1602', '1', '2022-11-26');

-- 2 task
insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '2', '160', '100', '1', '1', '2022-11-21');

insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '2', '100', '59', '1', '1', '2022-11-24');

insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '2', '59', '68', '1', '1', '2022-11-26');

insert into logs (organization_id, project_id, task_id, old_estimated_time, new_estimated_time, real_time, sprint_number, edit_date)
values ('1', '1', '2', '68', '0', '152', '1', '2022-11-28');
