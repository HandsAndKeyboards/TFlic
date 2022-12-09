-----------------------------------------------------------------------
-------------Developer--KastelProject--Yandex--------------------------
-----------------------------------------------------------------------
insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline) 
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'InProgress';
-----------------------------------------------------------------------
-------------Manager--KastelProject--Yandex--------------------------
-----------------------------------------------------------------------

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'InProgress';
-----------------------------------------------------------------------
-------------Supporter--KastelProject--Yandex--------------------------
-----------------------------------------------------------------------
insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Supporter' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Supporter' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Supporter' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Supporter' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Supporter' and prj.name = 'KastelProject' and org.name = 'Yandex' and c.name = 'InProgress';
-------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------------------------
-----------------------------------------------------------------------
-------------Developer--EmigrateProject--Yandex--------------------------
-----------------------------------------------------------------------
insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'InProgress';
-----------------------------------------------------------------------
-------------Manager--EmigrateProject--Yandex--------------------------
-----------------------------------------------------------------------
insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'EmigrateProject' and org.name = 'Yandex' and c.name = 'InProgress';
-------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------------------------
-----------------------------------------------------------------------
-------------Developer--SuperPuperProject--Apple--------------------------
-----------------------------------------------------------------------
insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Developer' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'InProgress';
-----------------------------------------------------------------------
-------------Manager--SuperPuperProject--Apple--------------------------
-----------------------------------------------------------------------
insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task1', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 2 , 'Task2', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task3', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'Done';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 0 , 'Task4', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'InProgress';

insert into tasks(column_id, position, name, description, creation_time, status, id_executor, deadline)
select c.id, 1 , 'Task5', 'Типичное описание', DATE '2022-04-04', 'done', 1, current_date
from columns as c
         inner join boards as b on b.id = c.board_id
         inner join projects as prj on b.project_id = prj.id
         inner join organizations as org on org.id = prj.organization_id
where b.name = 'Manager' and prj.name = 'SuperPuperProject' and org.name = 'Apple' and c.name = 'InProgress';


