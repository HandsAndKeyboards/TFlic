--Developer--KastelProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'KastelProject';

insert into columns(board_id, position, name) 
select b.id, 1 , 'TODO'
from boards as b
inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'KastelProject';
--Manager--KastelProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'KastelProject';
--Supporter--KastelProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Supporter' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Supporter' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Supporter' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Supporter' and prj.name = 'KastelProject';
--Doter--KastelProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Doter' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Doter' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Doter' and prj.name = 'KastelProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Doter' and prj.name = 'KastelProject';
--Developer--EmigrateProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'EmigrateProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'EmigrateProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'EmigrateProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'EmigrateProject';
--Manager--EmigrateProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'EmigrateProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'EmigrateProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'EmigrateProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'EmigrateProject';
--Developer--SuperPuperProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Developer' and prj.name = 'SuperPuperProject';
--Manager--SuperPuperProject
insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Manager' and prj.name = 'SuperPuperProject';
--Appler--SuperPuperProject

insert into columns(board_id, position, name)
select b.id, 0 , 'Backlog'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Appler' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 1 , 'TODO'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Appler' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 2 , 'InProgress'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Appler' and prj.name = 'SuperPuperProject';

insert into columns(board_id, position, name)
select b.id, 3 , 'Done'
from boards as b
         inner join projects as prj on b.project_id = prj.id
where b.name = 'Appler' and prj.name = 'SuperPuperProject';
